using AutoMapper;
using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Services
{
    public interface IRequestHandler
    {
        /// <summary>
        /// Get notifications
        /// </summary>
        /// <param name="id">employer</param>
        /// <returns>List of notifications</returns>
        Task<NotificationDto[]> GetNotifies(Guid id);
        /// <summary>
        /// Get reqests tht create employee
        /// </summary>
        /// <param name="id">employee id</param>
        /// <returns>request list</returns>
        Task<RequestDto[]> GetEmpoyeeRequest(Guid id);
        /// <summary>
        /// Get reqests directed to employer
        /// </summary>
        /// <param name="id">employer id</param>
        /// <returns>request list</returns>
        Task<RequestDto[]> GetEmpoyerRequest(Guid id);
    }

    public class RequestService : IRequestHandler
    {
        private readonly BaseDbContext _dbcontext;
        private readonly ILogger<RequestService> _logger;
        private readonly IMapper _mapper;
        private readonly string[] _requestsDescription =
        {
            ""
        };
        public RequestService(
            BaseDbContext dbcontext,
            ILogger<RequestService> logger,
            IMapper mapper) 
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<NotificationDto[]> GetNotifies(Guid id)
        {
            _logger.LogInformation("Find not signed requests of employer");
            var directedRequests = await _dbcontext.VacationRequests
                                                   .AsNoTracking()
                                                   .Include(r => r.Employee)
                                                   .Include(r => r.Type)
                                                   .Where(r => r.EmployerId == id &&
                                                               r.StatusId == (int)RequstsStatusesType.Direct)
                                                   .ToArrayAsync();
            var notifications = new NotificationDto[directedRequests.Length];
            _logger.LogInformation($"Finded requests {directedRequests.Length}");
            for (int i = 0; i < directedRequests.Length; i++)
            {
                notifications[i] = new NotificationDto
                {
                    RequestId = directedRequests[i].Id,
                    Message = $@"The request {directedRequests[i].Id} waits your approval. 
                        Initiator {string.Join(" ", directedRequests[i].Employee.FirstName, 
                                                    directedRequests[i].Employee.LastName)}. 
                        Vacation type {directedRequests[i].Type.Name}" 
                };
            }
            return notifications;
        }

        public async Task<RequestDto[]> GetEmpoyeeRequest(Guid id)
        {
            _logger.LogInformation($"Finding created request by employee {id}");
            var requests = await _dbcontext.VacationRequests
                                           .AsNoTracking()
                                           .Include(r => r.Employee)
                                           .Include(r => r.Type)
                                           .Where(r => r.EmployeeId == id)
                                           .ToArrayAsync();
            _logger.LogInformation($"Finded requests {requests.Length}");
            return _mapper.Map<RequestDto[]>(requests);
        }

        public async Task<RequestDto[]> GetEmpoyerRequest(Guid id)
        {
            var employer = await _dbcontext.Auths
                        .AsNoTracking()
                        .Include(a => a.RoleNavigation)
                        .Where(a => a.EmployeeId == id)
                        .SingleOrDefaultAsync();
            if (employer.RoleNavigation.Name != "employer")
            {
                _logger.LogError($"User {id} isn't employer");
                CVSApiException.ConcreteException(IncorrectDataType.NoRights);
                throw new CVSApiException();
            }
            _logger.LogInformation($"Finding created request of employer {id}");
            var requests = await _dbcontext.VacationRequests
                                           .AsNoTracking()
                                           .Include(r => r.Employee)
                                           .Include(r => r.Type)
                                           .Where(r => r.EmployerId == id)
                                           .ToArrayAsync();
            _logger.LogInformation($"Finded requests {requests.Length}");
            return _mapper.Map<RequestDto[]>(requests);
        }
    }
}
