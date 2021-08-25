using AutoMapper;
using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.DAL.Context;
using CalculationVacationSystem.DAL.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

    public class EmployeeServiceTest : EmployeeServiceTestSeed
    {
        private readonly DbConnection _connection;

        public EmployeeServiceTest()
            : base(
                new DbContextOptionsBuilder<BaseDbContext>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options)
        {
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        internal void Dispose() => _connection.Dispose();

        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<EmployeeService>> _loggerMock = new();

        [Fact]
        public async Task GetEmployeeInfo_UserNotFound_ThrowsException()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                var serv = new EmployeeService(context, _mapperMock.Object, _loggerMock.Object);
                var ex = await Assert.ThrowsAsync<CVSApiException>(async () => await serv.GetInfo(Guid.NewGuid()));
                Assert.Equal("There's no users with that id", ex.Message);
            }
        }

        [Fact]
        public async Task GetEmployeeInfo_FindUSer_ReturnEmployeeInfo()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                _mapperMock.Setup(x => x.Map<EmployeeInfoDto>((It.IsAny<Employee>())))
                       .Returns(_employee);
                var serv = new EmployeeService(context, _mapperMock.Object, _loggerMock.Object);
                var res = await serv.GetInfo(employeeId);
                var expEmpl = await context.Employees.Where(a => a.Id == employeeId).SingleOrDefaultAsync();
                _mapperMock.Verify(x => x.Map<EmployeeInfoDto>((It.IsAny<Employee>())), Times.Once);
                Assert.Equal(_mapperMock.Object.Map<EmployeeInfoDto>(expEmpl), res);
            }
        }
    }
    public class EmployeeServiceTestSeed : BaseServiceTest
    {
        protected EmployeeServiceTestSeed(DbContextOptions<BaseDbContext> contextOptions)
            : base(contextOptions) { }

        protected EmployeeInfoDto _employee;
        protected Guid employeeId = Guid.NewGuid();

        protected override void Seed()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                Guid structureId = Guid.NewGuid();
                Guid employerId = Guid.NewGuid();

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
                context.Employees.Add(new DAL.Entities.Employee
                {
                    Id = employerId,
                    FirstName = "chief",
                    LastName = "chief",
                    SecondName = "chief",
                    Position = "chief",
                    Email = "some@smtp.com",
                    EmploymentDate = DateTime.Now,
                    PersonalPhone = "+7897789789",
                    WorkPhone = "+4545645",
                    StructureId = structureId
                });


                context.Roles.Add(new DAL.Entities.Role
                {
                    Id = 1,
                    Name = "employer",
                    Description = ""
                });
                context.Roles.Add(new DAL.Entities.Role
                {
                    Id = 2,
                    Name = "chief",
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

                context.Auths.Add(new DAL.Entities.Auth
                {
                    EmployeeId = employerId,
                    Username = "chief",
                    Passhash = "a0Z3q8INCc32SyBiBpD+ccM13XJiGEBcKEvg214jWkyAOMAJD268jMSRbDkl4VEgVJ2o7jiuyJ6qxis6uGAwDQ==",
                    Salt = "sdff",
                    Role = 2
                });

                _employee = new EmployeeInfoDto
                {
                    FullName = "test test test",
                    ChiefFullName = "chief chief chief",
                    DepartName = "office"
                };

                context.SaveChanges();
            }
        }
    }
}
