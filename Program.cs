using ERP_System_Project.Extensions;
using ERP_System_Project.Middlewares;
using ERP_System_Project.Models;
using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Repository.Implementation;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Implementation;
using ERP_System_Project.Services.Implementation.Core;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Implementation.HR;
using ERP_System_Project.Services.Implementation.Inventory;
using ERP_System_Project.Services.Implementation.Log;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.Services.Interfaces.Log;
using ERP_System_Project.UOW;
using ERP_System_Project.Validators.HR;
using ERP_System_Project.Validators.Inventory;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers with Views
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<Erpdbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
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

// Memory Cache
builder.Services.AddMemoryCache();

// SMTP Email
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, EmailSender>();


// Validators Service
builder.Services.AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<WorkScheduleDayVMValidator>();

// Repositories and UnitOfWork
//builder.Services.AddDataSevices();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Core Services
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

// CRM Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerFavoriteService, CustomerFavoriteService>();
builder.Services.AddScoped<ICustomerReviewService, CustomerReviewService>();

// Inventory Services
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAttributeService, AttributeService>();

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

// Log Services
builder.Services.AddScoped<IPerformanceLogService, PerformanceLogService>();


// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

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

app.Run();