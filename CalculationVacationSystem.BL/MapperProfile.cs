using AutoMapper;
using CalculationVacationSystem.DAL.Entities;
using CalculationVacationSystem.WebApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Employee, EmployeeInfoDto>()
                .ForMember(d => d.FullName, opt =>
                    opt.MapFrom(src => $"{src.FirstName} {src.LastName} {src.SecondName}"))
                .ForMember(d => d.DepartName, opt =>
                    opt.MapFrom(src => src.Structure.Name));
        }
    }
}
