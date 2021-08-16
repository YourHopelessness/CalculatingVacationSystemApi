using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Dto
{
    public class NotificationDto
    {
        public Guid RequestId { get; set; }

        public string Message { get; set; }
    }
}
