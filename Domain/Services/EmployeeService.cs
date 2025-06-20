using AutoMapper;
using Core.Infrastruture.RepositoryPattern.Repository;
using Core.Infrastruture.UnitOfWork;
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
    public class EmployeeService(IUnitOfWork _unitOfWork, IMapper _mapper):IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository = _unitOfWork.Repository<Employee>();
        private readonly IRepository<Department> _departmentRepository = _unitOfWork.Repository<Department>();
        public async Task<EmployeeResult> CreateAsync(EmployeeDto employeeDto)
        {
            var result = new EmployeeResult();
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (await _departmentRepository.GetByIdAsync(employeeDto.DepartmentId) == null)
                    return Helper.Helper.CreateErrorResult<EmployeeResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Department"));
                if (employeeDto.ManagerId != null && await _employeeRepository.GetByIdAsync(employeeDto.ManagerId.Value) == null)
                    return Helper.Helper.CreateErrorResult<EmployeeResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Manager"));
               
                var employee = _mapper.Map<Employee>(employeeDto);
                if (employeeDto.ProfileImage != null)
                {
                    byte[] profileImageData = null;
                    using (var memoryStream = new MemoryStream())
                    {
                        await employeeDto.ProfileImage.CopyToAsync(memoryStream);
                        profileImageData = memoryStream.ToArray();
                    }

                    employee.ProfileImageExtension = Path.GetExtension(employeeDto.ProfileImage.FileName);
                    employee.ProfileImageLength = employeeDto.ProfileImage.Length;
                    employee.ProfileImage = profileImageData;
                }
              
                await _employeeRepository.AddAsync(employee);
                
                await _unitOfWork.CommitTransactionAsync();
                result.Employee = employeeDto;
                result.StatusCode = HttpStatusCode.Created;
                result.SuccessMessage = MessageEnum.Created(typeof(Employee).Name);
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Helper.Helper.CreateErrorResult<EmployeeResult>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<OperationResult> DeleteAsync(int id)
        {
            var result = new OperationResult();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null)
                    return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Employee"));
                var subordinates = await _employeeRepository.FindAllAsync(n => n.ManagerId == id);
                if (subordinates.Any())
                    return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.BadRequest, ErrorEnum.NotAvailableMessage("Employee has subordinates and cannot be deleted"));
                await _employeeRepository.DeleteAsync(employee);
                await _unitOfWork.CommitTransactionAsync();

                result.StatusCode = HttpStatusCode.OK;
                result.SuccessMessage = MessageEnum.Deleted(typeof(Employee).Name);
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Helper.Helper.CreateErrorResult<OperationResult>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<GetEmployeeWithoutImageResult> GetAllAsync()
        {
            var result = new GetEmployeeWithoutImageResult();

            var employees = await _employeeRepository.GetAllAsync(includeProperties: "Department,Manger");

            if (employees == null || !employees.Any())
                return Helper.Helper.CreateErrorResult<GetEmployeeWithoutImageResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Employees"));

            result.Employees = _mapper.Map<List<GetEmployeeWithoutImageDto>>(employees);

            foreach (var dto in result.Employees)
            {
                var employee = employees.FirstOrDefault(e => e.Id == dto.Id);
                if (employee?.Manger != null)
                {
                    dto.ManagerName = $"{employee.Manger.FirstName} {employee.Manger.LastName}";
                }
            }

            result.TotalCount = employees.Count;
            result.StatusCode = HttpStatusCode.OK;
            result.SuccessMessage = MessageEnum.Getted(typeof(Employee).Name);

            return result;
        }

        public async Task<GetListEmployeeResult> GetListEmployeesAsync()
        {
            var result = new GetListEmployeeResult();

            var employees = await _employeeRepository.GetAllAsync();

            if (!employees.Any())
                return Helper.Helper.CreateErrorResult<GetListEmployeeResult>(
                    HttpStatusCode.NotFound,
                    ErrorEnum.NotFoundAny("Employee"));

            result.Employees = employees.Select(e => new GetListEmployeeDto
            {
                Id = e.Id,
                Name = $"{e.FirstName} {e.LastName}"
            }).ToList();

            result.SuccessMessage = MessageEnum.Getted(nameof(employees));
            return result;
        }
        public async Task<GetEmployeeResult> GetAsync(int id)
        {
            var result = new GetEmployeeResult();
            var employee = await _employeeRepository.FindAsync(n=>n.Id== id,includeProperties:"Department,Manger");
            if (employee == null)
                return Helper.Helper.CreateErrorResult<GetEmployeeResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Employee"));
            result.Employee = _mapper.Map<GetEmployeeDto>(employee);
            result.StatusCode = HttpStatusCode.OK;
            result.SuccessMessage = MessageEnum.Getted(typeof(Employee).Name);
            return result;
        }
        public async Task<EmployeeResult> UpdateAsync(int id, EmployeeDto employeeDto)
        {
            var result = new EmployeeResult();
            try
            {
                var employee = await _employeeRepository.FindAsync(n => n.Id == id , includeProperties: "Department,Manger");
                if (employee == null)
                    return Helper.Helper.CreateErrorResult<EmployeeResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Employee"));

                if (await _departmentRepository.GetByIdAsync(employeeDto.DepartmentId) == null)
                    return Helper.Helper.CreateErrorResult<EmployeeResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Department"));

               
                if (employeeDto.ManagerId != null && await _employeeRepository.GetByIdAsync(employeeDto.ManagerId.Value) == null)
                    return Helper.Helper.CreateErrorResult<EmployeeResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Manager"));

                _mapper.Map(employeeDto, employee);
             
                if (employeeDto.ProfileImage != null)
                {
                    byte[] profileImageData = null;
                    using (var memoryStream = new MemoryStream())
                    {
                        await employeeDto.ProfileImage.CopyToAsync(memoryStream);
                        profileImageData = memoryStream.ToArray();
                    }

                    employee.ProfileImageExtension = Path.GetExtension(employeeDto.ProfileImage.FileName);
                    employee.ProfileImageLength = employeeDto.ProfileImage.Length;
                    employee.ProfileImage = profileImageData;
                }
                else
                {
                    employee.ProfileImage = null;
                    employee.ProfileImageExtension = null;
                    employee.ProfileImageLength = null;
                }
               
                await _employeeRepository.UpdateAsync(employee);
                await _unitOfWork.CommitTransactionAsync();
                result.Employee = employeeDto;
                result.StatusCode = HttpStatusCode.OK;
                result.SuccessMessage = MessageEnum.Updated(typeof(Employee).Name);
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Helper.Helper.CreateErrorResult<EmployeeResult>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<GetEmployeeResult> GetManagersEmployeesAsync(int id)
        {
            var result = new GetEmployeeResult();
         
                var employees =await _employeeRepository.FindAllAsync(n => n.ManagerId == id, includeProperties: "Department,Manger");
                if (employees == null)
                    return Helper.Helper.CreateErrorResult<GetEmployeeResult>(HttpStatusCode.NotFound, ErrorEnum.NotFoundMessage("Employees"));
                result.Employees = _mapper.Map<List<GetEmployeeDto>>(employees);
                result.StatusCode = HttpStatusCode.OK;
                result.SuccessMessage = MessageEnum.Getted(typeof(Employee).Name);
                return result;
        }
    }
}
