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
    public class RequestServiceTest : RequestServiceTestSeed
    {
        private readonly DbConnection _connection;

        public RequestServiceTest()
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

        public void Dispose() => _connection.Dispose();

        private readonly Mock<ILogger<RequestService>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();


        [Fact]
        public async Task GetNotifies_Employer_TwoNotification()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                var serv = new RequestService(context, _loggerMock.Object, _mapperMock.Object);
                var res = await serv.GetNotifies(employerId);
                Assert.Equal(2, res.Length);
            }
        }

        [Fact]
        public async Task GetNotifies_Employee_NoNotification()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                var serv = new RequestService(context, _loggerMock.Object, _mapperMock.Object);
                var res = await serv.GetNotifies(employeeId);
                Assert.Empty(res);
            }
        }

        [Fact]
        public async Task GetEmpoyeeRequest_TwoRequest()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                _mapperMock.Setup(x => x.Map<RequestDto[]>((It.IsAny<VacationRequest[]>())))
                      .Returns(requests);
                var serv = new RequestService(context, _loggerMock.Object, _mapperMock.Object);
                var res = await serv.GetEmpoyeeRequest(employeeId);
                _mapperMock.Verify(m => m.Map<RequestDto[]>(It.IsAny<VacationRequest[]>()), Times.Once);
            }
        }

        [Fact]
        public async Task GetEmpoyerRequest_Employer_TwoRequest()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                var serv = new RequestService(context, _loggerMock.Object, _mapperMock.Object);
                var res = await serv.GetEmpoyerRequest(employerId);
                _mapperMock.Verify(m => m.Map<RequestDto[]>(It.IsAny<VacationRequest[]>()), Times.Once);
            }
        }

        [Fact]
        public async Task GetEmpoyerRequest_Employee_ThrowsNoRights()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                var serv = new RequestService(context, _loggerMock.Object, _mapperMock.Object);
                var ex = await Assert.ThrowsAsync<CVSApiException>(async () => await serv.GetEmpoyerRequest(employeeId));
                Assert.Equal("User doesn't have rights to do this actions", ex.Message);
            }
        }
    }
    public class RequestServiceTestSeed : BaseServiceTest
    {
        protected RequestServiceTestSeed(DbContextOptions<BaseDbContext> contextOptions)
            : base(contextOptions) { }

        protected Guid employeeId = Guid.NewGuid();
        protected Guid employerId = Guid.NewGuid();
        protected RequestDto[] requests;

        protected override void Seed()
        {
            using (var context = new BaseDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                Guid structureId = Guid.NewGuid();
                
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

                Role employee = new DAL.Entities.Role
                {
                    Id = 1,
                    Name = "employee",
                    Description = ""
                };
                Role employer = new DAL.Entities.Role
                {
                    Id = 2,
                    Name = "employer",
                    Description = ""
                };
                context.Roles.Add(employee);
                context.Roles.Add(employer);

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

                context.VacationTypes.Add(new DAL.Entities.VacationType
                {
                    Id = 1,
                    Name = "test",
                    Description = "sds"
                });

                context.RequestStatuses.Add(new DAL.Entities.RequestStatus
                {
                    Id = 1,
                    Name = "Directed",
                    Description = "sds"
                });
                Guid idFrst = Guid.NewGuid();
                Guid idScnd = Guid.NewGuid();
                context.VacationRequests.Add(new DAL.Entities.VacationRequest
                { 
                    Id = idFrst,
                    EmployeeId = employeeId,
                    EmployerId = employerId,
                    DateChanged = DateTime.Now,
                    DateStart = DateTime.Now,
                    StatusId = 1,
                    Period = 10,
                    Reason = "asas",
                    TypeId = 1
                });

                context.VacationRequests.Add(new DAL.Entities.VacationRequest
                {
                    Id = idScnd,
                    EmployeeId = employeeId,
                    EmployerId = employerId,
                    DateChanged = DateTime.Now,
                    DateStart = DateTime.Now,
                    StatusId = 1,
                    Period = 45,
                    Reason = "sdfdsf",
                    TypeId = 1
                });

                requests = new RequestDto[]
                {
                    new RequestDto 
                    {
                        RequestId = idFrst,
                        Reason = "asas",
                        RequestStatus = "Directed",
                        Vacation = new VacationDto
                        {
                            EmployeeName = "test test test",
                            VacationPeriod = new TimeSpan(10, 0, 0, 0),
                            VacationType = "test"
                        }
                        },
                    new RequestDto
                    {
                        RequestId = idScnd,
                        Reason = "asas",
                        RequestStatus = "Directed",
                        Vacation = new VacationDto
                        {
                            EmployeeName = "test test test",
                            VacationPeriod =  new TimeSpan(10, 0, 0, 0),
                            VacationType = "test"
                        }
                    },
                };

                context.SaveChanges();
            }
        }
    }
}
