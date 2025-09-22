using ERP_System_Project.Extensions;
using ERP_System_Project.Models;
using ERP_System_Project.Repository.Implementation;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Implementation;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<Erpdbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// add Repositories and UnitOfWork
builder.Services.AddDataSevices();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // generic repo
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>)); // generic Service 

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Register CRM services
builder.Services.AddScoped<ICustomerService, CustomerService>();


// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
// add Email Service and SMTP
builder.Services.AddEmailService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
