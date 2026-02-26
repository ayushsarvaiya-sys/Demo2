using AutoMapper;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Mappings
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            // Employee Mappings
            CreateMap<Employee, EmployeeGetDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DesignationTitle, opt => opt.MapFrom(src => src.Designation.Title));
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();

            // Department Mappings
            CreateMap<Department, DepartmentGetDto>();
        }
    }
}
