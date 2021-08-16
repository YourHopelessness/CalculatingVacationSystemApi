using CalculationVacationSystem.DAL.Context;
using CalculationVacationSystem.BL.Dto;
using System;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using CalculationVacationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CalculationVacationSystem.BL.Utils;

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
        Task<EmployeeInfoDto> GetInfo(Guid id);

        /// <summary>
        /// Get all colleagues of current employee
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns>List of colleagues</returns>
        Task<IEnumerable<string>> GetAllColleagues(Guid id);
    }
    /// <summary>
    /// Employee service 
    /// </summary>
    public class EmloyeeService : IEmployeesServiceInterface
    {
        private readonly BaseDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwt;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dbcontext">database context</param>
        /// <param name="mapper">maps</param>
        /// <param name="jwt">maps</param>
        public EmloyeeService(BaseDbContext dbcontext, 
                              IMapper mapper,
                              IJwtUtils jwt)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _jwt = jwt;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<EmployeeInfoDto> GetInfo(Guid Id)
        {
            var user = await _dbcontext.Employees
                    .AsNoTracking()
                    .Include(c => c.Structure)
                    .SingleAsync(e => e.Id == Id);
            if (user == default(Employee))
            {
                CVSApiException.ConcreteException(IncorrectDataType.NoSuchUser);
                throw new CVSApiException();
            }

            var chief = 
                await _dbcontext.Auths
                                .Include(a => a.Employee)
                                .AsNoTracking()
                                .Where(e => e.Employee.StructureId == 
                                            user.StructureId && e.Role == 2)
                                .FirstOrDefaultAsync();
            var employeeInfo = new EmployeeInfoDto();
            employeeInfo = _mapper.Map<EmployeeInfoDto>(user);
            employeeInfo.ChiefFullName =
                String.Join(" ",
                            chief.Employee.FirstName,
                            chief.Employee.LastName,
                            chief.Employee.SecondName);
            return employeeInfo;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<IEnumerable<string>> GetAllColleagues(Guid id)
        {
            var user = await _dbcontext.Employees
                                .AsNoTracking()
                                .SingleOrDefaultAsync(a => a.Id == id);
            if (user == default(Employee))
            {
                CVSApiException.ConcreteException(IncorrectDataType.NoSuchUser);
                throw new CVSApiException();
            }
            return await _dbcontext.Employees
                                   .AsNoTracking()
                                   .Where(e => e.StructureId == user.StructureId)
                                   .Select(e => string.Join(" ",
                                                    e.FirstName,
                                                    e.LastName,
                                                    e.SecondName))
                                   .ToListAsync();
        }
    }
}
