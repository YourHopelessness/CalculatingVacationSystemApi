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
using System.Collections.Generic;
using CalculationVacationSystem.DAL.Entities;

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
                CVSApiException.ConcreteException(IncorrectDataType.Username);
                throw new CVSApiException();
            }
            var globalCrypt = new HMACSHA512(Encoding.ASCII.GetBytes(_configuration["GlobalSalt"]));
            var decPass = globalCrypt.ComputeHash(Encoding.ASCII.GetBytes(pass));
            var personalCrypt = new HMACSHA512(Encoding.ASCII.GetBytes(user.Salt));
            var Pass = Convert.ToBase64String(personalCrypt.ComputeHash(decPass));
            if (user.Passhash == Pass)
            {
                return _jwtTokenGen.GenerateJwtToken(
                        _mapper.Map<UserData>(user));
            }
            CVSApiException.ConcreteException(IncorrectDataType.Password);
            throw new CVSApiException();
        }

        /// <inheritdoc></inheritdoc>
        public async Task<UserData> GetById(Guid id)
        {
            var user = await _dbContext.Auths
                                       .AsNoTracking()
                                       .Include(a => a.Employee)
                                       .SingleOrDefaultAsync(a => a.EmployeeId == id);
            if (user == default(Auth))
            {
                CVSApiException.ConcreteException(IncorrectDataType.NoSuchUser);
                throw new CVSApiException();
            }
            return _mapper.Map<UserData>(user);
        }
    }
}
