using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace CalculationVacationSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthData _auth;
        public LoginController(IAuthData auth)
        {
            _auth = auth;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Authentificate([FromBody] AuthentificationDto userCredential)
        {
            var token = _auth.AuthentificateAsync(userCredential.Username, userCredential.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            SetTokenCookie(token.Result);
            return Ok(JsonSerializer.Serialize(token.Result));
        }

        private void SetTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("JWT", token, cookieOptions);
        }
    }
}
