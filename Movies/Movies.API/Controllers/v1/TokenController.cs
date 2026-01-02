using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.API.Controllers.v1
{
    /// <summary>
    /// Controller responsible for generating JWT tokens for accessing secured API endpoints for version 1.0 (Deprecated).
    /// Use version 2.0 for new developments.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration used to retrieve JWT settings.</param>
        public TokenController(IConfiguration configuration) => _configuration = configuration;

        /// <summary>
        /// Generates and returns a JSON Web Token (JWT) valid for 24 hours.
        /// </summary>
        /// <returns>An object containing the generated JWT token.</returns>
        /// <response code="200">Returns the generated token.</response>
        /// <response code="500">If the JWT secret is missing in configuration.</response>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetToken()
        {
            var secretKey = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secretKey)) return StatusCode(500, "JWT secret is missing in configuration.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "api-user"),
                new Claim(ClaimTypes.Role, "api-user"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}