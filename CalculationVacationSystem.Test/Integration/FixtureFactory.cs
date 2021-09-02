using CalculationVacationSystem.DAL.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.Test.Integration
{
    public class FixtureFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<BaseDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<BaseDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<BaseDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<FixtureFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder() =>
             base.CreateWebHostBuilder().UseEnvironment("Testing");

        public void InitializeDbForTests(BaseDbContext context)
        {
            Guid structureId = Guid.NewGuid();
            Guid employeeId = Guid.Parse("409c1d62-d80c-4f67-97f8-0846c4e31ffc");
            Guid employerId = Guid.Parse("a92d0258-98d1-4986-86b9-f14046af238f");
            Guid adminId = Guid.Parse("be5350df-3933-45a2-ae27-2a1525349261");

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
            context.Employees.Add(new DAL.Entities.Employee
            {
                Id = adminId,
                FirstName = "admin",
                LastName = "admin",
                SecondName = "admin",
                Position = "admin",
                Email = "some@smtp.com",
                EmploymentDate = DateTime.Now,
                PersonalPhone = "+7897789789",
                WorkPhone = "+4545645",
                StructureId = structureId
            });

            context.Roles.Add(new DAL.Entities.Role
            {
                Id = 1,
                Name = "employee",
                Description = ""
            });
            context.Roles.Add(new DAL.Entities.Role
            {
                Id = 2,
                Name = "employer",
                Description = ""
            });
            context.Roles.Add(new DAL.Entities.Role
            {
                Id = 3,
                Name = "admin",
                Description = ""
            });

            context.Auths.Add(new DAL.Entities.Auth
            {
                EmployeeId = employeeId,
                Username = "test",
                Passhash = "kXdI+0om3+6xCWEYhOpZh5b3IF4iEGYh/BoCNBHuy41Q8pMAXgA8YmQZffyB3Q9RjB72wxTlL4PABBYARydLaA==",
                Salt = "sdff",
                Role = 1
            });
            context.Auths.Add(new DAL.Entities.Auth
            {
                EmployeeId = employerId,
                Username = "chief",
                Passhash = "kXdI+0om3+6xCWEYhOpZh5b3IF4iEGYh/BoCNBHuy41Q8pMAXgA8YmQZffyB3Q9RjB72wxTlL4PABBYARydLaA==",
                Salt = "sdff",
                Role = 2
            });
            context.Auths.Add(new DAL.Entities.Auth
            {
                EmployeeId = adminId,
                Username = "admin", 
                Passhash = "kXdI+0om3+6xCWEYhOpZh5b3IF4iEGYh/BoCNBHuy41Q8pMAXgA8YmQZffyB3Q9RjB72wxTlL4PABBYARydLaA==",
                Salt = "sdff",
                Role = 3
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
            
            context.SaveChanges();
        }
    }
}
