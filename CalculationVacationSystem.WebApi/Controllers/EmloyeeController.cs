using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.WebApi.Attributes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        [Authorize]
        public async Task<EmployeeInfoDto> GetMyInfo() => await _employeeServices.GetMyInfo();

    }
}
