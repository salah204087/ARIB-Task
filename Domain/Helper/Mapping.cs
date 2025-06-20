using AutoMapper;
using DataLayer.Models;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap<Employee, GetEmployeeDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeDto, Employee>()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());
            CreateMap<Employee, GetEmployeeWithoutImageDto>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manger != null ? src.Manger.FirstName + " " + src.Manger.LastName : null))
               .ReverseMap();
            CreateMap<Department,GetDepartmentDto>()
                .ReverseMap();
            CreateMap<DepartmentDto, Department>()
                .ReverseMap();
            CreateMap<GetEmployeeDto, EmployeeDto>()
                .ReverseMap();
            CreateMap<DataLayer.Models.Task, TaskDto>().ReverseMap();
            CreateMap<DataLayer.Models.Task, GetTaskDto>().ReverseMap();

           
        }
    }
}
