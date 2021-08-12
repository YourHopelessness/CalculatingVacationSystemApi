using System;
using System.Collections.Generic;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class RequestStatus
    {
        public RequestStatus()
        {
            VacationRequests = new HashSet<VacationRequest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<VacationRequest> VacationRequests { get; set; }
    }
}
