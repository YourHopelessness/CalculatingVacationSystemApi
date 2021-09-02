using AutoMapper;
using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.DAL.Context;
using CalculationVacationSystem.DAL.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalculationVacationSystem.Test.Unit.Services
{
    public class AuthServiceTest : AuthServiceTestSeed, IDisposable
    {
        private readonly DbConnection _connection;

        public AuthServiceTest()
            : base(
                new DbContextOptionsBuilder<BaseDbContext>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options) => _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        public void Dispose() => _connection.Dispose();


        private Mock<IJwtUtils> _jwtTokenGenMock = new();
        private Mock<IConfiguration> _mockIConfiguration = new();
        private Mock<IMapper> _mapper = new();
        private Mock<ILogger<AuthService>> _mockLogger = new();

        [Fact]
        public async Task AuthentificateAsync_СorrectCredentials_ReturnToken()
        {
            _mockIConfiguration.Setup(x => x["GlobalSalt"]).Returns("abcdefghijklmnop");
            _mapper.SetReturnsDefault(_user);
            _jwtTokenGenMock.Setup(t => t.GenerateJwtToken(_user)).Returns("token");
            using (var context = new BaseDbContext(ContextOptions))
            {
                var authService = new AuthService(context, _jwtTokenGenMock.Object,
                            _mockIConfiguration.Object, _mapper.Object, _mockLogger.Object);
                var ex = await authService.AuthentificateAsync("test", "test");
                Assert.NotEmpty(ex);
            }
        }

        [Theory]
        [InlineData("notExist")]
        [InlineData("bla bla")]
        [InlineData("menya net")]
        public async Task AuthentificateAsync_IncorrectUsername_ThrowException(string username)
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                var authService = new AuthService(context, _jwtTokenGenMock.Object,
                            _mockIConfiguration.Object, _mapper.Object, _mockLogger.Object);
                var ex = await Assert.ThrowsAsync<CVSApiException>(() => authService.AuthentificateAsync(username, "sds"));
                Assert.Equal("The username is incorrect", ex.Message);
            }
        }

        [Theory]
        [InlineData("wrongPass")]
        [InlineData("nepravilnyi")]
        [InlineData("ne ygadal")]
        public async Task AuthentificateAsync_IncorrectPassword_ThrowException(string password)
        {
            _mockIConfiguration.Setup(x => x["GlobalSalt"]).Returns("abcdefghijklmnop");
            using (var context = new BaseDbContext(ContextOptions))
            {
                var authService = new AuthService(context, _jwtTokenGenMock.Object,
                            _mockIConfiguration.Object, _mapper.Object, _mockLogger.Object);
                var ex = await Assert.ThrowsAsync<CVSApiException>(() => authService.AuthentificateAsync("test", password));
                Assert.Equal("The password is incorrect", ex.Message);
            }
        }

        [Fact]
        public async Task GetById_NotFoundedUser_ThrowException()
        {
            Guid wrongId = Guid.NewGuid();
            using (var context = new BaseDbContext(ContextOptions))
            {
                var authService = new AuthService(context, _jwtTokenGenMock.Object,
                            _mockIConfiguration.Object, _mapper.Object, _mockLogger.Object);
                var ex = await Assert.ThrowsAsync<CVSApiException>(() => authService.GetById(wrongId));
                Assert.Equal("There's no users with that id", ex.Message);
            }
        }

        [Fact]
        public async Task GetById_ReturnUserData()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                _mapper = new();
                _mapper.Setup(x => x.Map<UserData>((It.IsAny<Auth>())))
                       .Returns(_user);
                var user = await context.Employees.Where(t => t.FirstName == "test").SingleOrDefaultAsync();
                var authService = new AuthService(context, _jwtTokenGenMock.Object,
                            _mockIConfiguration.Object, _mapper.Object, _mockLogger.Object);
                var res = await authService.GetById(user.Id);
                var expAuth = await context.Auths.Where(a => a.EmployeeId == user.Id).SingleOrDefaultAsync();
                _mapper.Verify(x => x.Map<UserData>((It.IsAny<Auth>())), Times.Once);
                Assert.Equal(_mapper.Object.Map<UserData>(expAuth), res);
            }
        }
    }

    public class AuthServiceTestSeed : BaseServiceTest
    {
        protected AuthServiceTestSeed(DbContextOptions<BaseDbContext> contextOptions)
            : base(contextOptions) { }
        protected UserData _user;

        protected override void Seed()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                Guid structureId = Guid.NewGuid();
                Guid employeeId = Guid.NewGuid();

                context.StructureUnits.Add(new DAL.Entities.StructureUnit
                {
                    Id = structureId,
                    ParentId = null,
                    Name = "office",
                    Address = "ulitsa pushkina dom kolotyshkina",
                    Code = "789",
                    Description = "test office"
                });

                context.Employees.Add(new DAL.Entities.Employee
                {
                    Id = employeeId,
                    FirstName = "test",
                    LastName = "test",
                    SecondName = "test",
                    Position = "test",
                    Email = "some@smtp.com",
                    EmploymentDate = DateTime.Now,
                    PersonalPhone = "+7897789789",
                    WorkPhone = "+4545645",
                    StructureId = structureId
                });
                context.Roles.Add(new DAL.Entities.Role
                {
                    Id = 1,
                    Name = "test",
                    Description = ""
                });

                context.Auths.Add(new DAL.Entities.Auth
                {
                    EmployeeId = employeeId,
                    Username = "test",
                    Passhash = "a0Z3q8INCc32SyBiBpD+ccM13XJiGEBcKEvg214jWkyAOMAJD268jMSRbDkl4VEgVJ2o7jiuyJ6qxis6uGAwDQ==",
                    Salt = "sdff",
                    Role = 1
                    
                });
                _user = new UserData { Id = employeeId, FullName = "test test test", Role = "admin" };
                context.SaveChanges();
            }
        }
    }
 }
