using AutoMapper;
using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.DAL.Context;
using CalculationVacationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Services
{
    public interface IAuthData
    {
        /// <summary>
        /// Authentificate user asynchronously
        /// </summary>
        /// <param name="username">usermame data</param>
        /// <param name="pass">entered password that needs to hash</param>
        /// <returns>token</returns>
        Task<string> AuthentificateAsync(string username, string pass);
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns>user data info</returns>
        Task<UserData> GetById(Guid id);
    }
    public class AuthService : IAuthData
    {
        private readonly BaseDbContext _dbContext;
        private readonly IJwtUtils _jwtTokenGen;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        public AuthService(BaseDbContext dbContext,
                           IJwtUtils jwtTokenGen,
                           IConfiguration configuration,
                           IMapper mapper,
                           ILogger<AuthService> logger)
        {
            _dbContext = dbContext;
            _jwtTokenGen = jwtTokenGen;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<string> AuthentificateAsync(string username, string pass)
        {
            _logger.LogInformation($"Finding user with username = {username}");
            var user = await _dbContext.Auths
                                        .AsQueryable()
                                        .Include(a => a.Employee)
                                        .Where(a => a.Username == username)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();
            if (user == null) // if there's no user with that username
            {
                _logger.LogError($"Username is not found");
                CVSApiException.ConcreteException(IncorrectDataType.Username);
                throw new CVSApiException();
            }

            _logger.LogInformation($"Validating password of user with username = {username}");
            var globalCrypt = new HMACSHA512(Encoding.ASCII.GetBytes(_configuration["GlobalSalt"]));
            var decPass = globalCrypt.ComputeHash(Encoding.ASCII.GetBytes(pass));
            var personalCrypt = new HMACSHA512(Encoding.ASCII.GetBytes(user.Salt));
            var Pass = Convert.ToBase64String(personalCrypt.ComputeHash(decPass));

            if (user.Passhash == Pass)
            {
                _logger.LogInformation($"Authetificate user {username}");
                return _jwtTokenGen.GenerateJwtToken(
                            _mapper.Map<UserData>(user));
            }
            _logger.LogError($"Password is not match");
            CVSApiException.ConcreteException(IncorrectDataType.Password);
            throw new CVSApiException();
        }

        /// <inheritdoc></inheritdoc>
        public async Task<UserData> GetById(Guid id)
        {
            _logger.LogInformation($"Finding user with id = {id}");
            var user = await _dbContext.Auths
                                       .AsNoTracking()
                                       .Include(a => a.Employee)
                                       .SingleOrDefaultAsync(a => a.EmployeeId == id);
            if (user == default(Auth))
            {
                _logger.LogError($"User not found");
                CVSApiException.ConcreteException(IncorrectDataType.NoSuchUser);
                throw new CVSApiException();
            }
            _logger.LogInformation($"Find user : {user.Employee.FirstName}");
            return _mapper.Map<UserData>(user);
        }
    }
}
