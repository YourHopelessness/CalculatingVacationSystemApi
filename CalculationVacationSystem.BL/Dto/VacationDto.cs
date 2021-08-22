using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Dto
{
    /// <summary>
    /// Vacation model
    /// </summary>
    public class VacationDto
    {
        public string EmployeeName { get; set; }

        public TimeSpan? VacationPeriod { get; set; }

        public string VacationType { get; set; }
    }
}
