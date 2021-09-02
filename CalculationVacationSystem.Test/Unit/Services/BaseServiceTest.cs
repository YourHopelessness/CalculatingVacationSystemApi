using CalculationVacationSystem.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.Test.Unit.Services
{
    public abstract class BaseServiceTest
    {
        protected DbContextOptions<BaseDbContext> ContextOptions { get; }
        protected BaseServiceTest(DbContextOptions<BaseDbContext> contextOptions)
        {
            ContextOptions = contextOptions; // set fake contexts optins based on real database context
            Seed(); //filling fake database
        }

        protected abstract void Seed();
    }
}
