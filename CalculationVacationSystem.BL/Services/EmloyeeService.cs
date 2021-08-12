using CalculationVacationSystem.DAL.Context;
using CalculationVacationSystem.WebApi.Dto;
using System;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using CalculationVacationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculationVacationSystem.BL
{
    public interface IEmployeesServiceInterface
    {
        Task<EmployeeInfoDto> GetMyInfo();
    }
    public class EmloyeeService : IEmployeesServiceInterface
    {
        private readonly BaseDbContext _dbcontext;
        private readonly IMapper _mapper;

        public EmloyeeService(BaseDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public Task<EmployeeInfoDto> GetMyInfo() => 
            Task.FromResult(_mapper.Map<Employee, EmployeeInfoDto>(
                    _dbcontext.Employees
                        .Include(c => c.Structure)
                        .Where(e => e.LastName == "Admin")
                        .AsNoTracking().FirstOrDefault()));
    }
}
