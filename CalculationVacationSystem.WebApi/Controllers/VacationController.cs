using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.WebApi.Attributes;

namespace CalculationVacationSystem.WebApi.Controllers
{
    /// <summary>
    /// Controller for vacation actions
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeCVS]
    public class VacationController
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public VacationController() { }

        /// <summary>
        /// Get vacation info
        /// </summary>
        /// <param name="yearType">current or next year info</param>
        /// <param name="dateStart">start showing period</param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<VacationDto[]> GetVacations(
            [FromQuery] VacationYearType yearType, 
            [FromQuery] DateTime? dateStart,
            [FromQuery] DateTime? dateEnd) 
        { throw new NotImplementedException("No reliazation"); }

        /// <summary>
        /// User takes a vacation
        /// </summary>
        /// <param name="vacation">Vacation info</param>
        /// <returns>null if success, else error message</returns>
        [HttpPost("[action]")]
        public async Task<string> PostTakeVacation([FromBody] VacationDto vacation)
            { throw new NotImplementedException("No reliazation"); }

        /// <summary>
        /// Edit vacation on next year
        /// </summary>
        /// <param name="vacation">vacation</param>
        /// <returns>>null if success, else error message</returns>
        [HttpPut("[action]")]
        public async Task<string> PutEditVacation([FromBody] VacationDto vacation)
        { throw new NotImplementedException("No reliazation"); }
    }
}
