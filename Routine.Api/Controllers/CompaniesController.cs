using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;


namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository , IMapper mapper)
        {
            _companyRepository = companyRepository ??
                throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[HttpGet]
        //public async Task<IActionResult> GetCompanies()
        //{
        //    var companies = await _companyRepository.GetCompaniesAsync();

        //    var companyDtos = new List<CompanyDto>();
        //    foreach (var company in companies)
        //    {
        //        companyDtos.Add(new CompanyDto
        //        {
        //            Id = company.Id,
        //            Name = company.Name
        //        });
        //    }

        //    return Ok(companyDtos);
        //}
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            //throw new Exception("just a exception");
            var companies = await _companyRepository.GetCompaniesAsync();

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);


            return Ok(companyDtos);
        }

        [HttpGet("{companyId}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
           
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CompanyDto>(company));
        }
    }
}
