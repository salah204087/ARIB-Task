using ARIB_Task_MVC.Interfaces;
using ARIB_Task_MVC.Services;
using Newtonsoft.Json;
using System.Security.Claims;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("ARIB_API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7004/api/");
});
builder.Services.AddScoped<IAccountService,AccountService>();
builder.Services.AddScoped<IDepartmentService,DepartmentService>();
builder.Services.AddScoped<IEmployeeService,EmployeeService>();
builder.Services.AddScoped<ITaskService,TaskService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    var rolesJson = context.Session.GetString("Roles");

    if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(rolesJson))
    {
        var roles = JsonConvert.DeserializeObject<List<string>>(rolesJson);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "LoggedUser")
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, "SessionAuth");
        context.User = new ClaimsPrincipal(identity);
    }

    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
