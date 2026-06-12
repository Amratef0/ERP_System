# 🏢 ERP System

A comprehensive **Enterprise Resource Planning (ERP)** web application built with **ASP.NET Core MVC (.NET 8)**. The system integrates multiple business modules including HR, Inventory, E-Commerce, CRM, and System Security — all under one platform with role-based permissions, background job scheduling, email notifications, and payment gateway integration.

> ⭐ The most advanced project in the portfolio — **581 commits**, **2 forks**, and enterprise-grade architecture.

---

## 🚀 Tech Stack

| Technology | Purpose |
|---|---|
| [ASP.NET Core MVC](https://learn.microsoft.com/en-us/aspnet/core/mvc/) (.NET 8) | Web framework |
| [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) | ORM & database access |
| [SQL Server](https://www.microsoft.com/en-us/sql-server) | Relational database |
| [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity) | Authentication & user management |
| [Hangfire](https://www.hangfire.io/) | Background job scheduling & dashboard |
| [FluentValidation](https://docs.fluentvalidation.net/) | Model validation |
| [AutoMapper](https://automapper.org/) | Object-to-object mapping |
| [SendGrid](https://sendgrid.com/) | Email notifications |
| [Paymob](https://paymob.com/) | Payment gateway integration |
| Google OAuth & Facebook OAuth | Social login (External Authentication) |
| C# / Razor Views | Backend logic & HTML templating |
| HTML5, CSS3, JavaScript | Frontend UI |

---

## 📁 Project Structure

```
ERP_System/
├── Controllers/              # MVC Controllers per module
├── Models/                   # EF Core entities & Identity models
├── ViewModels/               # View-specific data transfer objects
├── Services/
│   ├── Interfaces/
│   │   ├── Core/             # ICountryService, IBranchService, ICurrencyService
│   │   ├── CRM/              # ICustomerService, ICustomerAddressService, etc.
│   │   ├── ECommerce/        # IOfferService, ICartService, IOrderService
│   │   ├── HR/               # IEmployeeService, IAttendanceService, IPayrollRunService, etc.
│   │   ├── Inventory/        # IBrandService, ICategoryService, IProductService
│   │   ├── Log/              # IPerformanceLogService
│   │   └── System_Security/  # IUserService, PermissionService
│   └── Implementation/       # Concrete service implementations (mirrors Interfaces/)
├── Repository/
│   ├── Interfaces/           # IRepository<T>, IProductRepository, IUnitOfWork
│   └── Implementation/       # Generic & specific repository implementations
├── UOW/                      # Unit of Work pattern
├── Specification/            # Specification pattern for complex queries
├── BackgroundServices/       # IHostedService (ExpiredOfferBackgroundService)
├── Profiles/                 # AutoMapper mapping profiles
├── Validators/               # FluentValidation validators (HR, Inventory)
├── Middlewares/              # Custom middleware (PermissionMiddleware)
├── Extensions/               # Service registration extension methods
├── Helpers/                  # Utility & helper classes
├── Settings/                 # Strongly-typed settings (SendGrid, etc.)
├── Migrations/               # EF Core database migrations
├── Views/                    # Razor view templates
├── wwwroot/                  # Static files (CSS, JS, images)
├── Program.cs                # App entry point, DI registration & Hangfire jobs
├── appsettings.json          # Configuration & connection strings
└── ERP_System_Project.csproj # Project file & NuGet packages
```

---

## 🧩 ERP Modules

### 🌍 Core
Foundational reference data shared across all modules.
- Countries, Branches, Currencies

### 👥 CRM (Customer Relationship Management)
- Customer profiles and addresses
- Customer favorites, wishlists, and product reviews

### 📦 Inventory
- Brands and categories management
- Product catalog with attributes

### 🛒 E-Commerce
- Offers & promotions (with automatic expiry via background service)
- Shopping cart and order management
- Payment processing via **Paymob**

### 👨‍💼 HR (Human Resources)
Full HR lifecycle management:
- **Organization**: Departments, Job Titles, Employee Types, Work Schedules
- **Employees**: Employee records, Attendance tracking
- **Leaves**: Leave types, Leave policies, Leave balances, Leave requests
- **Payroll**: Payroll calculation, payroll entries, and automated monthly payroll runs
- **Public Holidays** management

### 📊 Logging
- Performance log tracking

### 🔐 System Security
- Role-based permission system with custom `PermissionMiddleware`
- User management via `IUserService`
- Reflection-based permission discovery via `ReflectionService`

---

## ⚙️ Prerequisites

- **Visual Studio 2022** or later
- **.NET 8 SDK** — [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (LocalDB, Express, or full edition)
- **SendGrid** account for email
- **Paymob** account for payments
- **Google** & **Facebook** developer app credentials (for social login)

---

## 🛠️ Installation

```bash
# 1. Clone the repository
git clone https://github.com/Amratef0/ERP_System.git
cd ERP_System
```

Open the solution `ERP_System_Project.sln` in **Visual Studio 2022**, then restore NuGet packages on build or run:

```bash
dotnet restore
```

---

## 🔧 Configuration

Update `appsettings.json` with your credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ERPSystemDb;Trusted_Connection=True;"
  },
  "SendGridSettings": {
    "ApiKey": "your_sendgrid_api_key",
    "SenderEmail": "your_email@example.com",
    "SenderName": "ERP System"
  },
  "Authentication": {
    "Google": {
      "ClientId": "your_google_client_id",
      "ClientSecret": "your_google_client_secret"
    },
    "Facebook": {
      "AppId": "your_facebook_app_id",
      "AppSecret": "your_facebook_app_secret"
    }
  }
}
```

---

## 🗄️ Database Setup

The app auto-migrates and seeds attendance records on startup. You can also run migrations manually:

In **Package Manager Console** (Visual Studio):

```powershell
Update-Database
```

Or via the .NET CLI:

```bash
dotnet ef database update
```

---

## ▶️ Running the App

```bash
dotnet run
```

Or press **F5** in Visual Studio. The app starts at `https://localhost:5001` and defaults to the **Market** controller.

Hangfire dashboard is accessible at:
```
https://localhost:5001/hangfire
```

---

## ⏰ Scheduled Background Jobs (Hangfire)

| Job | Schedule | Description |
|---|---|---|
| `GenerateNewYearLeaveBalances` | Jan 1st at 1:00 AM | Generates leave balances for all employees for the new year |
| `CarryForwardLeaveBalances` | Dec 31st at 11:59 PM | Carries forward unused leave days to the next year |
| `GenerateMonthlyPayroll` | Last day of month at 11:00 PM | Auto-generates the monthly payroll run |
| `ExpiredOfferBackgroundService` | Continuous (IHostedService) | Automatically expires outdated offers |

---

## 🏗️ Architecture & Design Patterns

The system applies multiple enterprise design patterns for maintainability and scalability:

```
Controller → Service Interface → Service Implementation
                 ↓
          Unit of Work (IUnitOfWork)
                 ↓
     Repository Interface → Repository Implementation
                 ↓
          DbContext (EF Core) → SQL Server
```

| Pattern | Usage |
|---|---|
| **Repository Pattern** | `IRepository<T>` / `Repository<T>` for generic data access |
| **Unit of Work** | `IUnitOfWork` / `UnitOfWork` to coordinate transactions |
| **Specification Pattern** | `Specification/` for complex, reusable query logic |
| **Service Layer** | Dedicated services per module (HR, CRM, Inventory, etc.) |
| **AutoMapper** | `Profiles/` for clean ViewModel ↔ Entity mapping |
| **FluentValidation** | `Validators/` for declarative input validation |
| **Custom Middleware** | `PermissionMiddleware` for route-level permission checks |

---

## 🔐 Authentication & Security

- **ASP.NET Core Identity** with email confirmation required
- **Social Login**: Google OAuth & Facebook OAuth
- **Session-based** security with HTTPS-only, SameSite=None cookies
- **CSRF protection** via AntiForgery tokens
- **Custom permission system** using `PermissionService` + `ReflectionService` + `PermissionMiddleware`
- **Token lifespan**: 2 hours for password reset / confirmation tokens

---

## 🤝 Contributing

Contributions are welcome!

1. Fork the repository
2. Create a new branch: `git checkout -b feature/your-feature`
3. Commit your changes with clear messages
4. Submit a pull request

---

## 📜 License

This project is open source under the **MIT License**.

---

## 👤 Author

**Amr Atef** — [@Amratef0](https://github.com/Amratef0)
