using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.Test.Unit.Controllers
{
    public class BaseControllerTest
    {
        protected HttpContext CreateHttpContext()
        {
            var responseMock = new Mock<HttpResponse>();
            responseMock.SetupGet(r => r.Headers).Returns(new HeaderDictionary());
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(a => a.Response).Returns(responseMock.Object);
            return httpContextMock.Object;
        }
    }
}
