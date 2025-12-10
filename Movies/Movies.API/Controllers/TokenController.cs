using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.API.Controllers
{
    /// <summary>
    /// Controller responsible for generating JWT tokens for accessing secured API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/>.
        /// </summary>
        /// <param name="configuration">Application configuration used to retrieve JWT settings.</param>
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates and returns a JSON Web Token (JWT) valid for 24 hours.
        /// This token can be used to authenticate and access protected API endpoints.
        /// </summary>
        /// <returns>An object containing the generated JWT token.</returns>
        [HttpGet]
        public IActionResult GetToken()
        {
            var secretKey = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secretKey))
                return StatusCode(500, "JWT secret is missing in configuration.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("role", "api-user")
            };

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds,
                claims: claims
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }
    }
}