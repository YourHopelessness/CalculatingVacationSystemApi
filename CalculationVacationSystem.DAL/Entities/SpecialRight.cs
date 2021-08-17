using System.Collections.Generic;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class SpecialRight
    {
        public SpecialRight()
        {
            EmployeeRights = new HashSet<EmployeeRight>();
            VacationTypeSpecialRights = new HashSet<VacationTypeSpecialRight>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<EmployeeRight> EmployeeRights { get; set; }
        public virtual ICollection<VacationTypeSpecialRight> VacationTypeSpecialRights { get; set; }
    }
}
