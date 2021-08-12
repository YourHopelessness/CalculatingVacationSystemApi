using System;
using System.Collections.Generic;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class StructureUnit
    {
        public StructureUnit()
        {
            Employees = new HashSet<Employee>();
            InverseParent = new HashSet<StructureUnit>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }
        public Guid? ParentId { get; set; }

        public virtual StructureUnit Parent { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<StructureUnit> InverseParent { get; set; }
    }
}
