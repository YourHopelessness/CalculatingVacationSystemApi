using System;
using System.Collections.Generic;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class VacationTypeSpecialRight
    {
        public int TypeId { get; set; }
        public int SpecialRightId { get; set; }
        public short MaxPeriod { get; set; }
        public bool IsNeedSign { get; set; }

        public virtual SpecialRight SpecialRight { get; set; }
        public virtual VacationType Type { get; set; }
    }
}
