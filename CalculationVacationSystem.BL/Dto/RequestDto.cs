using System;

namespace CalculationVacationSystem.BL.Dto
{
    public class RequestDto
    { 
         public Guid RequestId { get; set; }

         public VacationDto Vacation { get; set; }

         public string Reason { get; set; }

         public string RequestStatus { get; set; }

    }
}
