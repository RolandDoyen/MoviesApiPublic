using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movies.API.Profiles;
using Movies.BLL.BLL;
using Movies.BLL.Interfaces;
using Movies.BLL.Profiles;
using Movies.DAL;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Read JWT secret from configuration (appsettings.json)
    var jwtSecret = builder.Configuration["Jwt:Secret"];
    if (string.IsNullOrEmpty(jwtSecret))
        throw new Exception("JWT secret missing in configuration (Jwt:Secret).");

    // JWT Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ClockSkew = TimeSpan.Zero
        };
    });

    // CORS Policies
    builder.Services.AddCors(options =>
    {
        // Dev / testing : Allowing CORS
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });

        // Prod : Restrict CORS to frontend only
        options.AddPolicy("FrontendOnly", policy =>
        {
            policy.WithOrigins("https://movies-rd.azurewebsites.net")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // DB Context
    builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));

    // Dependency injections
    builder.Services.AddTransient<IMovieBLL, MovieBLL>();

    // AutoMapper
    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<MovieAPIProfile>();
        cfg.AddProfile<MovieBLLProfile>();
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Movies API",
            Description = @"<p><strong>Movies management API</strong></p>
                        <p>To test the endpoints:</p>
                        <ol>
                            <li>Make a <strong>GET</strong> request to <code>/api/v1/Token</code> to obtain your JWT token.</li>
                            <li>Click the <strong>Authorize</strong> button in Swagger and paste your token.</li>
                            <li>After authorization, you can test other API endpoints.</li>
                        </ol>",
            Contact = new OpenApiContact
            {
                Name = "Roland",
                Email = "roland.doyen@gmail.com"
            }
        });

        // JWT Configuration in Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token here"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

        // XML Comments
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseCors("AllowAll");
    }
    else
    {
        app.UseCors("FrontendOnly");
    }

    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies API V1");
        c.RoutePrefix = "swagger";
    });

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<DataContext>();
        db.Database.EnsureCreated();
    }

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error during application startup: {ex.Message}");
}