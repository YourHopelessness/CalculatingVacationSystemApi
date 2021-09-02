using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.BL.Utils;
using CalculationVacationSystem.WebApi;
using CalculationVacationSystem.WebApi.Controllers;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalculationVacationSystem.Test.Integration.Controllers
{
    public class EmoloyeeControllerTest : IClassFixture<FixtureFactory<Startup>>
    {
        private readonly FixtureFactory<Startup> _fixture;
        public EmoloyeeControllerTest(FixtureFactory<Startup> fixture)
        {
            _fixture = fixture;
        }

        private string GenerateToken(UserData user)
        {
            var gen = (IJwtUtils)_fixture.Services.GetService(typeof(IJwtUtils));
            return gen.GenerateJwtToken(user);
        }

        public static IEnumerable<object[]> TestData =>
           new List<object[]>
           {
                new object[] { new UserData { Id = Guid.Parse("409c1d62-d80c-4f67-97f8-0846c4e31ffc"), FullName = "test test test", Role = "employee"} },
                new object[] { new UserData { Id = Guid.Parse("a92d0258-98d1-4986-86b9-f14046af238f"), FullName = "chief chief chief", Role = "employer" } },
                new object[] { new UserData { Id = Guid.Parse("be5350df-3933-45a2-ae27-2a1525349261"), FullName = "admin admin admin", Role = "admin" } }
           };

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task GetMyInfo_Authorized_ReturnUserData(UserData user)
        {
            var httpClient = _fixture.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerateToken(user));
            var emplInfo = await httpClient.GetAsync("/api/Employee/GetMyInfo");
            Assert.True(emplInfo.IsSuccessStatusCode);
            var infoDto = 
                (EmployeeInfoDto)JsonConvert.DeserializeObject(await emplInfo.Content.ReadAsStringAsync(), typeof(EmployeeInfoDto));
            Assert.Equal(((UserData)TestData.ElementAt(1)[0]).FullName, infoDto.ChiefFullName);
            Assert.Equal("office", infoDto.DepartName);
            Assert.Contains(TestData.Select(x => ((UserData)x[0]).FullName), x => x == infoDto.FullName);
        }

        [Fact]
        public async Task GetMyInfo_Unauthorized()
        {
            var httpClient = _fixture.CreateClient();
            var emplInfo = await httpClient.GetAsync("/api/Employee/GetMyInfo");
            Assert.True(emplInfo.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetNotifies_Unauthorized()
        {
            var httpClient = _fixture.CreateClient();
            var emplInfo = await httpClient.GetAsync("/api/Employee/GetNotifies");
            Assert.True(emplInfo.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
