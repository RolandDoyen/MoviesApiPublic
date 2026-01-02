using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Movies.API.Middleware;
using Movies.Core.Exceptions;

namespace Movies.Tests.Integration.Middleware
{
    public class MiddlewareTestFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.Configure(app =>
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/ok", context =>
                    {
                        context.Response.ContentType = "text/plain";
                        return context.Response.WriteAsync("Everything is fine");
                    });
                    endpoints.MapGet("/throw-notfound", context =>
                    {
                        throw new MovieNotFoundException("Movie not found");
                    });
                    endpoints.MapGet("/throw-alreadyexists", context =>
                    {
                        throw new MovieAlreadyExistsException("Movie already exists");
                    });
                    endpoints.MapGet("/throw-generic", context =>
                    {
                        throw new Exception();
                    });
                });
            });
        }
    }
}