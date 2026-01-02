using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Movies.API.Profiles;
using Movies.BLL.Profiles;
using Xunit;

namespace Movies.Tests.Unit.AutoMapper
{
    public class AutoMapperTests
    {
        [Fact]
        public void AutoMapperConfiguration_IsValid()
        {
            var services = new ServiceCollection();
            services.AddLogging();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MovieAPIProfile>();
                cfg.AddProfile<MovieBLLProfile>();
            });

            var provider = services.BuildServiceProvider();
            var mapper = provider.GetRequiredService<IMapper>();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}