using System;
using System.Collections.Generic;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class Auth
    {
        public Guid EmployeeId { get; set; }
        public string Username { get; set; }
        public string Passhash { get; set; }
        public string Salt { get; set; }
        public int Role { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Role RoleNavigation { get; set; }
    }
}
