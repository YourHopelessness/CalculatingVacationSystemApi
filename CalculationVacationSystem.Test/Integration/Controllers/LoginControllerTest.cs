using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalculationVacationSystem.Test.Integration.Controllers
{
    public class LoginControllerTest : IClassFixture<FixtureFactory<Startup>>
    {
        private readonly FixtureFactory<Startup> _fixture;
        public LoginControllerTest(FixtureFactory<Startup> fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("chief", "test")]
        [InlineData("admin", "test")]
        public async Task Authentificate_Success(params string[] credentials)
        {
            var client = _fixture.CreateClient();
            JsonContent content = 
                JsonContent.Create(
                    new AuthenticationDto() { Username = credentials[0], Password = credentials[1]},
                    typeof(AuthenticationDto));
            var response = await client.PostAsync("/api/Login/Authentificate", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Authentificate_Failed_IncorrentCredentials()
        {
            var client = _fixture.CreateClient();
            JsonContent content =
                JsonContent.Create(
                    new AuthenticationDto() { Username = "test", Password = "asdsa" },
                    typeof(AuthenticationDto));
            var response = await client.PostAsync("/api/Login/Authentificate", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
