using ARIB_Task.Extensions;
using ARIB_Task.Middlewares;
using Core.Infrastruture.JWTValidation;
using DataLayer;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        #region Reference loop handling
        // builder.Services.AddControllers()
        //.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        #endregion

        #region DB connection
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

        });
        #endregion

        // Add services to the container.
        #region ActionFilters
        //builder.Services.AddMemoryCache();
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        { //Solve Json.Serialization error
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }).AddXmlDataContractSerializerFormatters();
        //builder.Services.AddHttpContextAccessor();
        #endregion
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        #region configures Swagger to add JWT authentication support to the Swagger UI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                    "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                    "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                    }
                     });

        });
        #endregion

        builder.Logging.AddConsole();

        #region Dependency Injection
        builder.Services.AddApplicationExtensions();

        #endregion
        builder.Services.AddCustomAuthentication(builder.Configuration);


        var app = builder.Build();




  
        app.MapOpenApi();
        
        #region Global Error Handler
        app.UseExceptionMiddleware();
        #endregion
        app.UseCors("AllowReactApp");
        #region authorization and authentication
        app.UseAuthentication();
        app.UseAuthorization();
        #endregion

        #region Routing
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        #endregion
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapControllers();


        app.Run();
    }
}