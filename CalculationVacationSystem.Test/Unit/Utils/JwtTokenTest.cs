using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace CalculationVacationSystem.Test.Unit.Services
{

    public class JwtTokenTest
    {
        private readonly Mock<IConfiguration> _configurationMock = new();
        private readonly Mock<ILogger<JwtTokenGenerator>> _loggerMock = new();
        private UserData _user = new UserData
        {
            Id = Guid.NewGuid(),
            FullName = "test test test",
            Role = "employee"
        };

        [Fact]
        public void GenerateToken_StringTokenReturn()
        {
            _configurationMock.Setup(x => x["JWT"]).Returns("testKeyBlaBlaBlaBlaBlaBlaBlaBlaBlaBla");
            var jwtGen = new JwtTokenGenerator(_configurationMock.Object, _loggerMock.Object);
            var res = jwtGen.GenerateJwtToken(_user);
            Assert.NotEmpty(res);
        }

        [Fact]
        public void ValidateToken_StringTokenReturn()
        {
            _configurationMock.Setup(x => x["JWT"]).Returns("testKeyBlaBlaBlaBlaBlaBlaBlaBlaBlaBla");
            var jwtGen = new JwtTokenGenerator(_configurationMock.Object, _loggerMock.Object);
            var res = jwtGen.ValidateJwtToken(jwtGen.GenerateJwtToken(_user));
            Assert.Equal(_user.Id, res);
        }

        [Fact]
        public void ValidateToken_nullToken_ReturnsNull()
        {
            var jwtGen = new JwtTokenGenerator(_configurationMock.Object, _loggerMock.Object);
            var res = jwtGen.ValidateJwtToken(null);
            Assert.Null(res);
        }

        [Fact]
        public void ValidateToken_InvalidToken_ReturnsNull()
        {
            _configurationMock.Setup(x => x["JWT"]).Returns("testKeyBlaBlaBlaBlaBlaBlaBlaBlaBlaBla");
            var jwtGen = new JwtTokenGenerator(_configurationMock.Object, _loggerMock.Object);
            var res = jwtGen.ValidateJwtToken("ghjkladjfdsla;ddjffsladjdfkslajddkla;sjdakl;JSDKAL;ksjdfkal;asksjdakl;JSADKL");
            Assert.Null(res);
        }
    }
}
