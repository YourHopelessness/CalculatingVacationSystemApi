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
        Task<NotificationDto[]> GetNotifies(Guid id);
    }

    public class RequestService : IRequestHandler
    {
        private readonly BaseDbContext _dbcontext;
        private readonly ILogger<RequestService> _logger;
        public RequestService(
            BaseDbContext dbcontext,
            ILogger<RequestService> logger) 
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        public async Task<NotificationDto[]> GetNotifies(Guid id)
        {
            var directedRequests = await _dbcontext.VacationRequests
                                                   .AsNoTracking()
                                                   .Include(r => r.Employee)
                                                   .Include(r => r.Type)
                                                   .Where(r => r.EmployerId == id &&
                                                               r.StatusId == (int)RequstsStatusesType.Direct)
                                                   .ToArrayAsync();
            var notifications = new NotificationDto[directedRequests.Length];
            for (int i = 0; i < directedRequests.Length; i++)
            {
                notifications[i] = new NotificationDto
                {
                    RequestId = directedRequests[i].Id,
                    Message = $@"The request {directedRequests[i].Id} waits your approval. 
                        Initiator {string.Join(" ", directedRequests[i].Employee.FirstName, directedRequests[i].Employee.LastName)}. 
                        Vacation type {directedRequests[i].Type.Name}" 
                };
            }
            return notifications;
        }
    }
}
