using System;
using System.Collections.Generic;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeRights = new HashSet<EmployeeRight>();
            VacationRequestEmployees = new HashSet<VacationRequest>();
            VacationRequestEmployers = new HashSet<VacationRequest>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string PersonalPhone { get; set; }
        public string WorkPhone { get; set; }
        public DateTime EmploymentDate { get; set; }
        public Guid StructureId { get; set; }
        public string LastName { get; set; }

        public virtual StructureUnit Structure { get; set; }
        public virtual Auth Auth { get; set; }
        public virtual ICollection<EmployeeRight> EmployeeRights { get; set; }
        public virtual ICollection<VacationRequest> VacationRequestEmployees { get; set; }
        public virtual ICollection<VacationRequest> VacationRequestEmployers { get; set; }
    }
}
