using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.DAL.Context;
using CalculationVacationSystem.BL.Utils;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using AutoMapper;

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
        Task<EmployeeInfoDto> GetById(Guid id);
    }
    public class AuthService : IAuthData
    {
        private readonly BaseDbContext _dbContext;
        private readonly IJwtUtils _jwtTokenGen;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(BaseDbContext dbContext,
                           IJwtUtils jwtTokenGen,
                           IConfiguration configuration,
                           IMapper mapper)
        {
            _dbContext = dbContext;
            _jwtTokenGen = jwtTokenGen;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<string> AuthentificateAsync(string username, string pass)
        {
            var user = await _dbContext.Auths
                                        .AsQueryable()
                                        .Include(a => a.Employee)
                                        .Where(a => a.Username == username)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();
            if (user == null) // if there's no user with that username
            {
                WebException.ConcreteException(IncorrectDataType.Username);
                throw new WebException();
            }
            var globalCrypt = new HMACSHA512(Encoding.ASCII.GetBytes(_configuration["GlobalSalt:Salt"]));
            var decPass = globalCrypt.ComputeHash(Encoding.ASCII.GetBytes("admin"));
            var personalCrypt = new HMACSHA512(Encoding.ASCII.GetBytes(user.Salt));
            var Pass = Convert.ToBase64String(personalCrypt.ComputeHash(decPass));
            if (user.Passhash == Pass)
            {
                return _jwtTokenGen.GenerateJwtToken(
                        _mapper.Map<UserData>(user));
            }
            WebException.ConcreteException(IncorrectDataType.Password);
            throw new WebException();
        }

        public async Task<EmployeeInfoDto> GetById(Guid id)
        {
            var user = await _dbContext.Employees.Where(a => a.Id == id).AsNoTracking().FirstOrDefaultAsync();
            return _mapper.Map<EmployeeInfoDto>(user);
        }

       
    }
}
