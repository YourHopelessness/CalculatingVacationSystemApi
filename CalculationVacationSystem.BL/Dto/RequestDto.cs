using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Dto
{
    public class RequestDto
    {
        Guid RequestId { get; set; }

        public string RequestHead { get; set; }

        public string RequestBody { get; set; }

        public string RequestStatus { get; set; }
    }
}
