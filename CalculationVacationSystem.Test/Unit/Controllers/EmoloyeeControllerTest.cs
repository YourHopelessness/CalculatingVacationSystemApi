using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.BL.Services;
using CalculationVacationSystem.WebApi.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalculationVacationSystem.Test.Unit.Controllers
{
    public class EmoloyeeControllerTest : BaseControllerTest
    {
       

        [Fact]
        public async Task GetMyInfo_NotFoundedUser()
        {

        }

        [Fact]
        public void GetNotifiesUnauthorized()
        {

        }

        [Fact]
        public void EditUserInfoAuthorizedAdmin()
        {

        }

        [Fact]
        public void EditUserInfoUnauthorized()
        {

        }

        [Fact]
        public void EditUserInfoForbidden()
        {

        }
    }
}
