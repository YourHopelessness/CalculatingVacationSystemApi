using AutoMapper;
using CalculationVacationSystem.DAL.Entities;
using CalculationVacationSystem.BL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationVacationSystem.BL.Utils
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
            CreateMap<Auth, UserData>()
                 .ForMember(d => d.FullName, opt =>
                    opt.MapFrom(src => String.Join(" ",
                            src.Employee.FirstName,
                            src.Employee.LastName,
                            src.Employee.SecondName)))
                 .ForMember(d => d.Id, opt => opt.MapFrom(src => src.EmployeeId));

        }
    }
}
