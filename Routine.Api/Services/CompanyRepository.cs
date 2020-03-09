using Microsoft.EntityFrameworkCore;
using Routine.Api.DbContexts;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository 
    {
        private readonly RoutineDbContext _context;

        public CompanyRepository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            company.Id = Guid.NewGuid();
            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }
            
            _context.Companies.Add(company);
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {

            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.CompanyId = companyId;
            _context.Employees.Add(employee);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.AnyAsync(x => x.Id == companyId);
        }

        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _context.Companies.Remove(company);

        }

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters )
        {
            if (parameters == null )
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            //if (string.IsNullOrWhiteSpace(parameters.SearchTerm) && 
            //    string.IsNullOrWhiteSpace(parameters.CompanyName))
            //{
            //    return await _context.Companies.ToListAsync();
            //}
            var queryExpression = _context.Companies as IQueryable<Company>;

            if (! string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                parameters.CompanyName = parameters.CompanyName.Trim();
                queryExpression = queryExpression.Where(x => x.Name == parameters.CompanyName);
            }
            if (! string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                parameters.SearchTerm = parameters.SearchTerm.Trim();
                queryExpression = queryExpression.Where(x => x.Name.Contains(parameters.SearchTerm) ||
                                                            x.Introduction.Contains(parameters.SearchTerm));
            }
            //queryExpression = queryExpression.Skip(parameters.PageSize * (parameters.PageNumber - 1))
            //    .Take(parameters.PageSize);

            //return await queryExpression.ToListAsync();
            return await PagedList<Company>.CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {

            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }
            return await _context.Companies.Where(x => companyIds.Contains(x.Id))
                .OrderBy(a => a.Name)
                .ToListAsync();

        }

        public async Task<Company> GetCompanyAsync(Guid companyId)
        {

            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }
            return await _context.Employees
                .Where(x => x.CompanyId == companyId && x.Id == employeeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId , string genderDisplay , string q )
        {

            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (string.IsNullOrWhiteSpace(genderDisplay) && string.IsNullOrWhiteSpace(q))
            {
                return await _context.Employees
               .Where(x => x.CompanyId == companyId)
               .OrderBy(x => x.EmployeeNo)
               .ToListAsync();
            }
            var items = _context.Employees.Where(x => x.CompanyId == companyId);

            if (! string.IsNullOrWhiteSpace(genderDisplay))
            {
                genderDisplay = genderDisplay.Trim();
                var gender = Enum.Parse<Gender>(genderDisplay);

                items = items.Where(x => x.Gender == gender);
            }
            if (! string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                items = items.Where(x => x.EmployeeNo.Contains(q) ||
                x.FirstName.Contains(q) ||
                x.LastName.Contains(q));
            }

            //var genderStr = genderDisplay.Trim();
            //var gender = Enum.Parse<Gender>(genderStr);

            return await items
                .OrderBy(x => x.EmployeeNo)
                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) >= 0;
        }

        public void UpdateCompany(Company company)
        {
            _context.Update(company);
        }

        public void UpdateEmployee(Employee employee)
        {
            //_context.Update(employee);
            // dbContextEntityFraworkCore  Entity被查询出来后 属性被跟踪 
            //属性变化之后 dbContext知道了 
            //执行 saveChanges方法 变化就写入数据库

            /*
             * 采用repository模式
             * 做到与存储无关
             * 写Controller时仅关心业务逻辑
             * repository相当于进行了一层抽象
             * 做成了一些合约
             * 让程序员仅关心合约（业务）
             * 降低程序的耦合性
             * 适用单元测试
             */
        }
    }
}
