using CalculationVacationSystem.BL.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalculationVacationSystem.WebApi.Middleware
{
    /// <summary>
    /// Error handler
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next">next method in chain</param>
        public ErrorHandlerMiddleware(RequestDelegate next) => _next = next;

        /// <summary>
        /// Middleware calls
        /// </summary>
        /// <param name="context">current Http context</param>
        /// <returns>async task</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // next in chain
            }
            catch (Exception error) // if exception occurs
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    CVSApiException => (int)HttpStatusCode.BadRequest, // handled while the data is wrong
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,   // not found page 
                    _ => (int)HttpStatusCode.InternalServerError, // unexpected error
                };
                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }

    }

}
