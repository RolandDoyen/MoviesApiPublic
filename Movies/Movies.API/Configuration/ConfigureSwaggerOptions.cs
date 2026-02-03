using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.API.Configuration
{
    /// <summary>
    /// Configures Swagger generation options to support API versioning and UI metadata.
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The provider used to discover and describe API versions.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        /// <summary>
        /// Configures the Swagger options by creating a documentation for each discovered API version.
        /// </summary>
        /// <param name="options">The Swagger generation options to configure.</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        /// <summary>
        /// Creates the specific metadata (Title, Version, Description) for a given API version.
        /// </summary>
        /// <param name="description">The version description used to build the metadata.</param>
        /// <returns>An <see cref="OpenApiInfo"/> object containing the versioned API details.</returns>
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Movies API",
                Version = description.ApiVersion.ToString(),
                Description = @$"<p><strong>Movies management API</strong></p>
                            <p>To test the endpoints:</p>
                            <ol>
                                <li>Make a <strong>GET</strong> request to <code>/api/{description.GroupName}/Token</code> to obtain your JWT token.</li>
                                <li>Click the <strong>Authorize</strong> button in Swagger and paste your token.</li>
                                <li>After authorization, you can test other API endpoints.</li>
                            </ol>",
                Contact = new OpenApiContact { Name = "Roland", Email = "roland.doyen@gmail.com" }
            };

            if (description.IsDeprecated) info.Description += "\n\n ## ⚠️ WARNING: This version is deprecated";

            return info;
        }
    }
}
