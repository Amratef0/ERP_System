using ERP_System_Project.BackgroundServices;
using ERP_System_Project.Extensions;
using ERP_System_Project.Middlewares;
using ERP_System_Project.Models;
using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Repository.Implementation;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Implementation;
using ERP_System_Project.Services.Implementation.Core;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Implementation.ECommerce;
using ERP_System_Project.Services.Implementation.HR;
using ERP_System_Project.Services.Implementation.Inventory;
using ERP_System_Project.Services.Implementation.Log;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.Services.Interfaces.Log;
using ERP_System_Project.UOW;
using ERP_System_Project.Validators.HR;
using ERP_System_Project.Validators.Inventory;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers with Views
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<Erpdbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Hangfire
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add the Hangfire server
builder.Services.AddHangfireServer();

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
})
    .AddEntityFrameworkStores<Erpdbcontext>()
    .AddDefaultTokenProviders();

// Authentication (Google)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    options.DefaultChallengeScheme = "Google";
})
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
})
.AddFacebook("Facebook", options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
});
// Token lifespan
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(2);
});

// Antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "AntiForgeryCookie";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.HttpOnly = true;
});

// Cookie Policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

// Session
builder.Services.AddSession(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.HttpOnly = true;
});
builder.Services.AddHttpContextAccessor();

// Memory Cache
builder.Services.AddMemoryCache();

// SMTP Email
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, EmailSender>();


// Validators Service
builder.Services.AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<WorkScheduleDayVMValidator>();

// Seed Service
builder.Services.AddScoped<SeedService>();


// Repositories and UnitOfWork
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Background Services
builder.Services.AddHostedService<ExpiredOfferBackgroundService>();

// Core Services
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

// CRM Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerFavoriteService, CustomerFavoriteService>();
builder.Services.AddScoped<ICustomerWishlistService, CustomerWishlistService>();
builder.Services.AddScoped<ICustomerReviewService, CustomerReviewService>();

// Inventory Services
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAttributeService, AttributeService>();

// ECommerce Services
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<ICartService, CartService>();

// HR Services
builder.Services.AddScoped<IWorkScheduleService, WorkScheduleService>();
builder.Services.AddScoped<IWorkScheduleDayService, WorkScheduleDayService>();
builder.Services.AddScoped<IPublicHolidayService, PublicHolidayService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeTypeCodeService, AttendanceStatusCodeService>();
builder.Services.AddScoped<IEmployeeTypeService, EmployeeTypeService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<IJobTitleService, JobTitleService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<ILeavePolicyService, LeavePolicyService>();
builder.Services.AddScoped<IEmployeeLeaveBalanceService, EmployeeLeaveBalanceService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();

// Log Services
builder.Services.AddScoped<IPerformanceLogService, PerformanceLogService>();


// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();


// Seed Database (Attendance Records)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<Erpdbcontext>();
    db.Database.Migrate();

    var seeder = services.GetRequiredService<SeedService>();

    await seeder.SeedAttendanceRecordsAsync();
}

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

// ========== SCHEDULED BACKGROUND JOBS ==========

// 1. LEAVE BALANCE: Generate balances for new year (January 1st at 1:00 AM)
RecurringJob.AddOrUpdate<IEmployeeLeaveBalanceService>(
    "GenerateNewYearLeaveBalances",
    service => service.GenerateBalancesForAllEmployeesAsync(DateTime.Now.Year, null),
    "0 1 1 1 *", // Cron: At 1:00 AM on January 1st
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });

// 2. LEAVE BALANCE: Carry forward unused balances (December 31st at 11:59 PM)
RecurringJob.AddOrUpdate<IEmployeeLeaveBalanceService>(
    "CarryForwardLeaveBalances",
    service => service.CarryForwardUnusedDaysAsync(DateTime.Now.Year, DateTime.Now.Year + 1),
    "59 23 31 12 *", // Cron: At 11:59 PM on December 31st
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });

// Schedule Recurring Jobs (Attendance Generation) - COMMENTED OUT
//var times = new[] { 9, 13, 18 };

//foreach (var hour in times)
//{
//    string jobId = $"GenerateAttendance_{hour}";
//    string cron = $"0 {hour} * * *";

//    RecurringJob.AddOrUpdate<AttendanceService>(
//        jobId,
//        service => service.GenerateDailyAttendance(),
//        cron);
//}

// Configure HTTP pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseMiddleware<EndpointPerformanceMiddleware>();

await app.RunAsync();