using AutoMapper;
using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.DAL.Context;
using CalculationVacationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly ILogger<EmloyeeService> _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dbcontext">database context</param>
        /// <param name="mapper">maps</param>
        /// <param name="logger">logger</param>
        public EmloyeeService(BaseDbContext dbcontext,
                              IMapper mapper,
                              ILogger<EmloyeeService> logger)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<EmployeeInfoDto> GetInfo(Guid Id)
        {
            _logger.LogInformation($"Finding user with id = {Id}");
            var user = await _dbcontext.Employees
                    .AsNoTracking()
                    .Include(c => c.Structure)
                    .SingleAsync(e => e.Id == Id);
            if (user == default(Employee))
            {
                _logger.LogError($"User not found");
                CVSApiException.ConcreteException(IncorrectDataType.NoSuchUser);
                throw new CVSApiException();
            }

            _logger.LogInformation($"Getting user info, id = {Id}");
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
            _logger.LogInformation($"Finding user with id = {id}");
            var user = await _dbcontext.Employees
                                .AsNoTracking()
                                .SingleOrDefaultAsync(a => a.Id == id);
            if (user == default(Employee))
            {
                _logger.LogError($"User not found");
                CVSApiException.ConcreteException(IncorrectDataType.NoSuchUser);
                throw new CVSApiException();
            }
            _logger.LogInformation($"Getting colleagues of user, id = {id}");
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
