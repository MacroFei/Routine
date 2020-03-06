using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeesController(IMapper mapper , ICompanyRepository companyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId)
        //{
        //    if (!await _companyRepository.CompanyExistsAsync(companyId))
        //    {
        //        return NotFound();
        //    }
        //    var employees = _companyRepository.GetEmployeesAsync(companyId);

        //    //var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        //    var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        //    return Ok(employeeDtos);

        //}
        [HttpGet(Name = nameof(GetEmployeesForCompany))]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>>
            GetEmployeesForCompany(Guid companyId,
            [FromQuery(Name = "gender")] string genderDisplay,
            string q )
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employees = await _companyRepository
                .GetEmployeesAsync(companyId , genderDisplay , q);

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        [HttpGet("{employeeId}" , Name =nameof(GetEmployeeForCompany))]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompany(Guid companyId , Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee =await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }

        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId ,EmployeeAddDto employee)
        {
            if (! await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var entity = _mapper.Map<Employee>(employee);
            _companyRepository.AddEmployee(companyId, entity);
            await _companyRepository.SaveAsync();

            var dtoToReturn = _mapper.Map<EmployeeDto>(entity);

            return CreatedAtRoute(nameof(GetEmployeeForCompany),new 
            { 
                companyId,
                employeeId = dtoToReturn.Id
            },dtoToReturn);  
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(
            Guid companyId,
            Guid employeeId,
            EmployeeUpdateDto employee)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);

            if (employeeEntity == null)
            {
                return NotFound();
            }

            // entity 转化为 updateDto
            // 把传进来的employee的值更新到 updateDto
            // 把updateDto映射回entity

            _mapper.Map(employee, employeeEntity);

            _companyRepository.UpdateEmployee(employeeEntity);

            await _companyRepository.SaveAsync();

            //204
            return NoContent();
        }
    }
}
