using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CalculationVacationSystem.WebApi.Controllers
{
    /// <summary>
    /// Request controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeCVS]
    public class RequestController : Controller
    {
        private readonly IRequestHandler _request;
        public RequestController(IRequestHandler request)
        {
            _request = request;
        }

        /// <summary>
        /// Get all request of employee
        /// </summary>
        /// <returns>list of requests</returns>
        [HttpGet("[action]")]
        public async Task<RequestDto[]> GetMyRequests() =>
           await _request.GetEmpoyeeRequest(((UserData)HttpContext.Items["User"]).Id);

        /// <summary>
        /// Get request for approval
        /// </summary>
        /// <returns>list of request</returns>
        [HttpGet("[action]")]
        [AuthorizeCVS(Role = "employer")]
        public async Task<RequestDto[]> GetApprovals() =>
            await _request.GetEmpoyerRequest(((UserData)HttpContext.Items["User"]).Id);


        /// <summary>
        /// Approve request
        /// </summary>
        /// <param name="requestId">id of approved request</param>
        [HttpPut("[action]")]
        [AuthorizeCVS(Role = "employer")]
        public async Task PutApproveRequest([FromBody] Guid requestId)
        { }

        /// <summary>
        /// Dissaprove request
        /// </summary>
        /// <param name="requestId">id of request</param>
        [HttpPut("[action]")]
        [AuthorizeCVS(Role = "employer")]
        public async Task PutDissaproveRequest([FromBody] Guid requestId)
        { }

        /// <summary>
        /// When request is viewed
        /// </summary>
        /// <param name="requestId">id of request</param>
        [HttpPut("[action]")]
        public async Task PutViewedRequest([FromBody] Guid requestId)
        { }
    }
}
