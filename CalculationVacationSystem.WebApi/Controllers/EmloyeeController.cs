using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.WebApi.Attributes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculationVacationSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    [AuthorizeCVS]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeesServiceInterface _employeeServices;
        private readonly IRequestHandler _requestService;
        public EmployeeController(
            IEmployeesServiceInterface employeeServices,
            IRequestHandler requestService)
        {
            _employeeServices = employeeServices;
            _requestService = requestService;
        }

        [HttpGet("[action]")]
        public async Task<EmployeeInfoDto> GetMyInfo() =>
            await _employeeServices.GetInfo(((UserData)HttpContext.Items["User"]).Id);

        [HttpGet("[action]")]
        public async Task<IEnumerable<string>> GetColleaguesNames() =>
            await _employeeServices.GetAllColleagues(((UserData)HttpContext.Items["User"]).Id);

        [HttpGet("[action]")]
        public async Task<NotificationDto[]> GetNotifies() =>
            await _requestService.GetNotifies(((UserData)HttpContext.Items["User"]).Id);

        [HttpPut("[action]")]
        [AuthorizeCVS(Role = "admin")]
        public async Task EditUserInfo([FromBody] Guid id) { throw new NotImplementedException("No reliazation"); }
    }
}
