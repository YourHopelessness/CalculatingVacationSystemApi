using CalculationVacationSystem.BL.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace CalculationVacationSystem.WebApi.Attributes
{
    #nullable enable
    /// <summary>
    /// Custom authorize attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeCVSAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Roles user have to have
        /// </summary>
        public string? Role { get; set; }
        /// <summary>
        /// when aurorize
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                return; // for anonym-access methods
            }

            // authorization
            var user = (UserData?)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            if (Role != null && user?.Role != Role)
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                return;
            }
        }

    }

}
