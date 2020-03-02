using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;

        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository ??
                throw new ArgumentNullException(nameof(companyRepository));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyRepository.GetCompaniesAsync();

            var companyDtos = new List<CompanyDto>();
            foreach (var company in companies)
            {
                companyDtos.Add(new CompanyDto
                {
                    Id = company.Id,
                    Name = company.Name
                });
            }

            return Ok(companyDtos);
        }

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetCompany(Guid companyId)
        {
           
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }
    }
}
