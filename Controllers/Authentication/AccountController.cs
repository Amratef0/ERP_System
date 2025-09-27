using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Web;
using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;


namespace ERP_System_Project.Controllers.Authentication
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailSender;
        private readonly ICustomerService _customerService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IEmailService emailSender,
        ICustomerService  customerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View("Login");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRegister(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Register", model);

            var appuser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName= model.LastName,
                Email = model.Email,
                PhoneNumber= model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                CreatedAt= DateTime.Now,
            };


            var result = await _userManager.CreateAsync(appuser, model.Password);

            if (!result.Succeeded)
            {
                var existingUser = await _userManager.FindByNameAsync(appuser.UserName);

                if (existingUser != null && !await _userManager.IsEmailConfirmedAsync(existingUser))
                {
                    appuser = existingUser;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Register", model);
                }
            }
           await _customerService.CreateCustomerByApplicationUserAsync(appuser, model);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appuser);

            var confirmationLink = Url.Action("ConfirmEmailToken", "Account",
                new { userId = appuser.Id, token = token }, Request.Scheme);

            await _emailSender.SendEmailAsync(
                model.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>."
            );

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(appuser, "User");

            return View("RegisterConfirmation");
        }


        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailToken(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return View("Register");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Register");
            }


            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return View("Register");
            }

            return View("ConfirmEmailSuccess");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLogin(LoginUserViewModel UserViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appuser = null;

                if (UserViewModel.UserNameOrEmail.Contains("@"))
                {
                    appuser = await _userManager.FindByEmailAsync(UserViewModel.UserNameOrEmail);
                }
                else
                {
                    appuser = await _userManager.FindByNameAsync(UserViewModel.UserNameOrEmail);
                }

                if (appuser != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(appuser, UserViewModel.Password);
                    if (found)
                    {
                        await _signInManager.SignInAsync(appuser, UserViewModel.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "User Or Password Wrong");
            }
            return View("Login", UserViewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveForgotPassword(ForgotPasswordViewModel
model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View(model);
                }

                var token = await
_userManager.GeneratePasswordResetTokenAsync(user);

                await _emailSender.SendEmailAsync(model.Email, "Password Reset Token",
                    $"Your reset token is: {token}");

                TempData["Email"] = model.Email;
                TempData["GeneratedToken"] = token;

                return RedirectToAction("EnterToken");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EnterToken()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SaveEnterToken(EnterTokenViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                var token = TempData["GeneratedToken"] as string;

                if (email == null || token == null)
                {
                    ModelState.AddModelError("", "Token expired. Please try again.");
                    return View(model);
                }

                if (model.Token == token)
                {
                    TempData["UserEmail"] = email;
                    TempData["ResetToken"] = token;
                    return RedirectToAction("ResetPassword");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid token");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            var email = TempData["UserEmail"] as string;
            var token = TempData["ResetToken"] as string;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveResetPassword(ResetPasswordViewModel
model)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPassword", model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("ForgotPassword");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token,
model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "✅ Your password has been successfully reset!";
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("ResetPassword", model);
        }
        [HttpGet]
public IActionResult GoogleLogin(string returnUrl = "/")
{
    var redirectUrl = Url.Action("GoogleResponse", "Account", new { ReturnUrl = returnUrl });
    var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
    return Challenge(properties, "Google");
}





[HttpGet]
public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
{
    var info = await _signInManager.GetExternalLoginInfoAsync();
    if (info == null)
        return RedirectToAction("Login");

    // محاولة تسجيل دخول باستخدام بيانات خارجية
    var signInResult = await _signInManager.ExternalLoginSignInAsync(
        info.LoginProvider,
        info.ProviderKey,
        isPersistent: false);

    if (!signInResult.Succeeded)
    {
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (email != null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };
                await _userManager.CreateAsync(user);

                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                await _userManager.AddToRoleAsync(user, "User");
            }

            var existingLogins = await _userManager.GetLoginsAsync(user);
            if (!existingLogins.Any(l => l.LoginProvider == info.LoginProvider))
            {
                await _userManager.AddLoginAsync(user, info);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
        }
    }

    return LocalRedirect(returnUrl);
}
    }
}

