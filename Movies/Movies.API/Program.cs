using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movies.API.Configuration;
using Movies.API.Middleware;
using Movies.API.Profiles;
using Movies.BLL.BLL;
using Movies.BLL.Interfaces;
using Movies.BLL.Profiles;
using Movies.DAL;
using Movies.DAL.Interfaces;
using Movies.DAL.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Read JWT secret from configuration (appsettings.json)
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is missing in appsettings.json");

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
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });

    builder.Services.AddAuthorization();

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
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            }
        )
    );

    // Dependency injections
    builder.Services.AddTransient<IMovieBLL, MovieBLL>();
    builder.Services.AddScoped<IMovieRepository, MovieRepository>();

    // AutoMapper
    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<MovieAPIProfile>();
        cfg.AddProfile<MovieBLLProfile>();
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // API Versioning
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    // Swagger
    builder.Services.AddSwaggerGen(c =>
    {
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

    // Exception Middleware
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseRouting();

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
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
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

public partial class Program { }