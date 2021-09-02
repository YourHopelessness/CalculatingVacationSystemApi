using CalculationVacationSystem.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalculationVacationSystem.Test.Integration.Controllers
{
    public class BaseControllerTest
         : IClassFixture<FixtureFactory<Startup>>
    {
        private readonly FixtureFactory<Startup> _factory;

        public BaseControllerTest(FixtureFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/Employee/GetMyInfo")]
        [InlineData("/api/Employee/GetNotifies")]
        [InlineData("/api/Request/GetMyRequests")]
        [InlineData("/api/Request/GetApprovals")]
        [InlineData("/api/Vacation/GetVacations")]
        public async Task Get_Endpoints_ReturnSuccessUnauthorizedForbidAndJson(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                        response.StatusCode == System.Net.HttpStatusCode.OK ||
                        response.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.Equal("application/json; charset=utf-8",
                    response.Content.Headers.ContentType.ToString());
        }
    }
}
