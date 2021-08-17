using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalculationVacationSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly IAuthData _auth;
        public LoginController(IAuthData auth)
        {
            _auth = auth;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Authentificate([FromBody] AuthentificationDto userCredential)
        {
            var token = await _auth.AuthentificateAsync(userCredential.Username, userCredential.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            SetTokenCookie(token);
            return Ok(JsonSerializer.Serialize(token));
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("JWT", token, cookieOptions);
        }
    }
}
