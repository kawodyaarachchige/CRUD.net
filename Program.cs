using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFirstApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the DbContext with a connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21)))); // Adjust the version accordingly

// Configure authentication
builder.Services.AddAuthentication("AdminAuth")
    .AddCookie("AdminAuth", options =>
    {
        options.LoginPath = "/Admin/Login"; // Redirect here if not authenticated
        options.AccessDeniedPath = "/Admin/Login"; // Redirect here if access denied
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); 


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}");

app.Run();