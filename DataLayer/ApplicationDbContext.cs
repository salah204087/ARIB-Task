using DataLayer.Helpers;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace DataLayer
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            //self-referencing relationship(Employee)
            modelBuilder.Entity<Employee>()
            .HasOne(c => c.Manger)
            .WithMany(c => c.Employees)
            .HasForeignKey(c => c.ManagerId)
            .OnDelete(DeleteBehavior.Restrict); // Avoid circular delete issues

            modelBuilder.Entity<ApplicationRole>().HasData(
               new ApplicationRole
               {
                   Id = (int)RoleEnum.Admin,
                   Name = RoleEnum.Admin.ToString(),
                   NormalizedName = RoleEnum.Admin.ToString().ToUpper()
               },
                new ApplicationRole
                {
                    Id = (int)RoleEnum.Employee,
                    Name = RoleEnum.Employee.ToString(),
                    NormalizedName = RoleEnum.Employee.ToString().ToUpper()
                },
                new ApplicationRole
                {
                    Id = (int)RoleEnum.Manager,
                    Name = RoleEnum.Manager.ToString(),
                    NormalizedName = RoleEnum.Manager.ToString().ToUpper()
                }
               );
        }
    }
}
