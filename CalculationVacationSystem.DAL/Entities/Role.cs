using System;
using System.Collections.Generic;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class Role
    {
        public Role()
        {
            Auths = new HashSet<Auth>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Auth> Auths { get; set; }
    }
}
