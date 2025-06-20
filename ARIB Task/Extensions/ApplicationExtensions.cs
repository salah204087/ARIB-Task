using ARIB_Task.Middlewares;
using Core.Infrastruture.RepositoryPattern.Repository;
using Core.Infrastruture.UnitOfWork;
using DataLayer;
using Domain.Helper;
using Domain.Interfaces;
using Domain.Services;
using System.Reflection;

namespace ARIB_Task.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationExtensions(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
           
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<ExceptionMiddleware>();
            services.AddAutoMapper(typeof(Mapping));

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowFrontApp", builder =>
            //    {
            //        builder.WithOrigins("*")
            //                       .AllowAnyMethod()
            //                       .AllowAnyHeader()
            //                       .AllowCredentials();
            //    });
            //});

            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITaskService, TaskService>();


            services.AddScoped(typeof(IUnitOfWork), services =>
            {
                return services.GetRequiredService<ApplicationDbContext>();
            });
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            return services;
        }
    }

}
