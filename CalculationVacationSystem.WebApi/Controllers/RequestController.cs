using CalculationVacationSystem.WebApi.Attributes;
using CalculationVacationSystem.BL.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculationVacationSystem.WebApi.Controllers
{
    /// <summary>
    /// Request controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeCVS]
    public class RequestController
    {
        public RequestController() { }

        /// <summary>
        /// Get all request of employee
        /// </summary>
        /// <returns>list of requests</returns>
        [HttpGet("[action]")]
        public async Task<RequestDto[]> GetMyRequests()
        { throw new NotImplementedException("No reliazation"); }

        /// <summary>
        /// Get request for approval
        /// </summary>
        /// <returns>list of request</returns>
        [HttpGet("[action]")]
        [AuthorizeCVS(Role = "employer")]
        public async Task<RequestDto[]> GetApprovals()
        { throw new NotImplementedException("No reliazation"); }

        /// <summary>
        /// Approve request
        /// </summary>
        /// <param name="requestId">id of approved request</param>
        [HttpPut("[action]")]
        [AuthorizeCVS(Role = "employer")]
        public async Task PutApproveRequest([FromBody] Guid requestId)
        { throw new NotImplementedException("No reliazation"); }

        /// <summary>
        /// Dissaprove request
        /// </summary>
        /// <param name="requestId">id of request</param>
        [HttpPut("[action]")]
        [AuthorizeCVS(Role = "employer")]
        public async Task PutDissaproveRequest([FromBody] Guid requestId)
        { throw new NotImplementedException("No reliazation"); }

        /// <summary>
        /// When request is viewed
        /// </summary>
        /// <param name="requestId">id of request</param>
        [HttpPut("[action]")]
        public async Task PutViewedRequest([FromBody] Guid requestId)
        { throw new NotImplementedException("No reliazation"); }
    }
}
