using AutoMapper;
using Core.Infrastruture.RepositoryPattern.Repository;
using DataLayer.Models;
using Domain.Common;
using Domain.DTOs;
using Domain.Helper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class DepartmentService(IRepository<Department> _departmentRepository,IRepository<Employee> _employeeRepository, IMapper _mapper):IDepartmentService
    {
        public async Task<GetDepartmentResult> GetAllAsync()
        {
            var result = new GetDepartmentResult();

            var departments = await _departmentRepository.GetAllAsync(includeProperties:"Employees");
            if (departments is null || !departments.Any())
                return Helper.Helper.CreateErrorResult<GetDepartmentResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundAny(nameof(Department)));

            var departmentsMapped = _mapper.Map<List<GetDepartmentDto>>(departments);

            result.Departments = departmentsMapped;
            result.SuccessMessage = MessageEnum.Getted("Department");
            return result;
        }
        public async Task<GetDepartmentResult> GetByIdAsync(int id)
        {
            var result = new GetDepartmentResult();

            var departments = await _departmentRepository.FindAsync(n=>n.Id==id, includeProperties: "Employees");
            if (departments is null)
                return Helper.Helper.CreateErrorResult<GetDepartmentResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundAny(nameof(Department)));

            var departmentsMapped = _mapper.Map<GetDepartmentDto>(departments);

            result.Department = departmentsMapped;
            result.SuccessMessage = MessageEnum.Getted(nameof(departments));

            return result;
        }
        public async Task<DepartmentResult> CreateAsync(DepartmentDto departmentDto)
        {
            var result = new DepartmentResult();

            if (departmentDto.Name.StartsWith(' ') || string.IsNullOrEmpty(departmentDto.Name))
                return Helper.Helper.CreateErrorResult<DepartmentResult>(HttpStatusCode.BadRequest, "invalid name");

            if (_departmentRepository.Exist(x => x.Name.ToLower() == departmentDto.Name.ToLower()))
                return Helper.Helper.CreateErrorResult<DepartmentResult>(HttpStatusCode.Conflict, "Department name already exist");

            var department = _mapper.Map<Department>(departmentDto);
            await _departmentRepository.AddAsync(department);

            result.StatusCode = HttpStatusCode.Created;
            result.Department = departmentDto;
            result.SuccessMessage = MessageEnum.Created(nameof(Department));
            return result;
        }
        public async Task<DepartmentResult> UpdateAsync(int id, DepartmentDto departmentDto)
        {
            var result = new DepartmentResult();

            if (departmentDto.Name.StartsWith(' '))
                return Helper.Helper.CreateErrorResult<DepartmentResult>(HttpStatusCode.BadRequest, "Department name can't start with space");


            if (await _departmentRepository.FindAsync(x=>x.Id == id) is not { } department)
                return Helper.Helper.CreateErrorResult<DepartmentResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundAny(nameof(Department)));



            if (_departmentRepository.Exist(x => x.Name.ToLower() == departmentDto.Name.ToLower() && x.Id != department.Id))
                return Helper.Helper.CreateErrorResult<DepartmentResult>(HttpStatusCode.Conflict, "Department name already exist");


            _mapper.Map(departmentDto, department);
            await _departmentRepository.UpdateAsync(department);

            result.Department = departmentDto;
            result.SuccessMessage = MessageEnum.Created(nameof(Department));
            return result;
        }
        public async Task<OperationResult> DeleteAsync(int id)
        {
            var result = new OperationResult();

            if (await _departmentRepository.FindAsync(x => x.Id == id) is not { } department)
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundAny(nameof(Department)));

            if (await _employeeRepository.FindAllAsync(x => x.DepartmentId == id) is { Count: > 0 })
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.Conflict, "You can't delete this department because it has employees");

            await _departmentRepository.DeleteAsync(department);

            result.SuccessMessage = MessageEnum.Deleted(nameof(Department));
            return result;
        }

    }
}
