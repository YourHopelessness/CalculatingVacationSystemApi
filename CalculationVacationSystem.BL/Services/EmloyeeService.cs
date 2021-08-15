using CalculationVacationSystem.DAL.Context;
using CalculationVacationSystem.BL.Dto;
using System;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using CalculationVacationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculationVacationSystem.BL.Services
{
    /// <summary>
    /// Interface for getting and saving info about employees
    /// </summary>
    public interface IEmployeesServiceInterface
    {
        /// <summary>
        /// Get information about current user
        /// </summary>
        /// <returns></returns>
        Task<EmployeeInfoDto> GetMyInfo();
    }
    /// <summary>
    /// Employee service 
    /// </summary>
    public class EmloyeeService : IEmployeesServiceInterface
    {
        private readonly BaseDbContext _dbcontext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dbcontext">database context</param>
        /// <param name="mapper">maps</param>
        public EmloyeeService(BaseDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<EmployeeInfoDto> GetMyInfo() =>
            _mapper.Map<Employee, EmployeeInfoDto>(
                await _dbcontext.Employees
                    .Include(c => c.Structure)
                    .Where(e => e.LastName == "Admin") // лучать Id из сервиса вутентификации
                    .AsNoTracking().SingleAsync());
    }
}
