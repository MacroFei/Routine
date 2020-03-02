using Microsoft.EntityFrameworkCore;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.DbContexts
{
    public class RoutineDbContext : DbContext
    {
        public RoutineDbContext( DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>()
                .Property(x => x.Introduction).IsRequired().HasMaxLength(500);

            modelBuilder.Entity<Employee>()
                .Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>()
                .Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>()
                .Property(x => x.LastName).IsRequired().HasMaxLength(50);


            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Company)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("7644b71d-d74e-43e2-ac32-8cbadd7b1c3a"),
                    Name = "Microsoft",
                    Introduction = "Great Company"
                },
                new Company
                {
                    Id = Guid.Parse("679dfd33-32e4-4393-b061-f7abb8956f53"),
                    Name="Google",
                    Introduction = "No Evil Company..."
                },
                new Company
                {
                    Id =Guid.Parse("1861341e-b42b-410c-ae21-cf11f36fc574"),
                    Name="Alipapa",
                    Introduction="Fubao Company"
                }) ;
        }

    }
}
