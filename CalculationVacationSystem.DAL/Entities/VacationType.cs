using System.Collections.Generic;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class VacationType
    {
        public VacationType()
        {
            VacationRequests = new HashSet<VacationRequest>();
            VacationTypeSpecialRights = new HashSet<VacationTypeSpecialRight>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<VacationRequest> VacationRequests { get; set; }
        public virtual ICollection<VacationTypeSpecialRight> VacationTypeSpecialRights { get; set; }
    }
}
