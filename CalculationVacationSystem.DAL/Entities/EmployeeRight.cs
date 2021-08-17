using System;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class EmployeeRight
    {
        public Guid EmployeeId { get; set; }
        public int SpecialRightId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual SpecialRight SpecialRight { get; set; }
    }
}
