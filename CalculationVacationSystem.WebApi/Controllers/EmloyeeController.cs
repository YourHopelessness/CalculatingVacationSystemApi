using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculationVacationSystem.BL;
using CalculationVacationSystem.WebApi.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalculationVacationSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeesServiceInterface _employeeServices;
        public EmployeeController(IEmployeesServiceInterface employeeServices)
        {
            _employeeServices = employeeServices;
        }

        [HttpGet("[action]")]
        public async Task<EmployeeInfoDto> GetMyInfo() => await _employeeServices.GetMyInfo();

        [HttpOptions("{id}")]
        public IActionResult PreflightRoute(int id)
        {
            return NoContent();
        }

        // OPTIONS: api/TodoItems2 
        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            return NoContent();
        }
    }
}
