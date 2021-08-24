using AutoMapper;
using CalculationVacationSystem.BL.Dto;
using CalculationVacationSystem.DAL.Entities;
using System;

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
                 .ForMember(d => d.Id, opt => opt.MapFrom(src => src.EmployeeId))
                 .ForMember(d => d.Role, opt =>
                    opt.MapFrom(src => src.RoleNavigation.Name));
            CreateMap<VacationRequest, VacationDto>()
                .ForMember(d => d.EmployeeName, opt =>
                    opt.MapFrom(src => String.Join(" ",
                            src.Employee.FirstName,
                            src.Employee.LastName,
                            src.Employee.SecondName)))
                .ForMember(d => d.VacationType, opt =>
                    opt.MapFrom(src => src.Type.Name))
                .ForMember(d => d.VacationPeriod, opt =>
                    opt.MapFrom(src => new TimeSpan(src.Period, 0, 0, 0)));
            CreateMap<VacationRequest, RequestDto>()
                .ForMember(d => d.RequestStatus, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(d => d.RequestId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
