using System;

#nullable disable

namespace CalculationVacationSystem.DAL.Entities
{
    public partial class VacationRequest
    {
        public Guid Id { get; set; }
        public DateTime DateStart { get; set; }
        public short Period { get; set; }
        public int TypeId { get; set; }
        public Guid EmployeeId { get; set; }
        public int StatusId { get; set; }
        public Guid EmployerId { get; set; }
        public string Reason { get; set; }
        public DateTime DateChanged { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Employee Employer { get; set; }
        public virtual RequestStatus Status { get; set; }
        public virtual VacationType Type { get; set; }
    }
}
