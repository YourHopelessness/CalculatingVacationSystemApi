using CalculationVacationSystem.BL.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CalculationVacationSystem.BL.Utils
{
    /// <summary>
    /// Interface that declare method for jwt tokens
    /// </summary>
    public interface IJwtUtils
    {
        /// <summary>
        /// Generate new jwt token and user's claim
        /// </summary>
        /// <param name="user">user info</param>
        /// <returns>generate token</returns>
        string GenerateJwtToken(UserData user);
        /// <summary>
        ///Validete jwt token when authorization is needs
        /// </summary>
        /// <param name="token">user token</param>
        /// <returns>guid of finded user or null if user is not founded</returns>
        Guid? ValidateJwtToken(string token);
    }
    /// <summary>
    /// Implements IJwtUtils interface 
    /// </summary>
    public class JwtTokenGenerator : IJwtUtils
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenGenerator> _logger;
        public JwtTokenGenerator(IConfiguration configuration,
                                 ILogger<JwtTokenGenerator> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateJwtToken(UserData user)
        {
            _logger.LogInformation($"Generate new token for user {user.FullName}");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1), // expires tomorrow
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key),
                                           SecurityAlgorithms.HmacSha256Signature) // signed by HS256
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); //create new user token
            _logger.LogInformation($"User token is registrted: token = {token}");
            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidateJwtToken(string token)
        {
            if (token == null)
            {
                _logger.LogInformation($"User token is null. User is unauthorized");
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            _logger.LogInformation($"Validating user token...");
            var key = Encoding.ASCII.GetBytes(_configuration["JWT"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var d = jwtToken.Claims.Where(x => x.Type == "id").FirstOrDefault();
                var userId = Guid.Parse(d.Value);
                _logger.LogInformation($"User token is validate. User id = {userId}");
                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                _logger.LogError($"User token is invalid");
                return null;
            }
        }
    }
}
