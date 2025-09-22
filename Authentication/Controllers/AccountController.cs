using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using ERP_System_Project.Models;
using ERP_System_Project.Interface;
using System.Net;
using System.Web;


namespace ERP_System_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ERP_System_Project.Interface.IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ERP_System_Project.Interface.IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
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

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "This email is already registered.");
                return View("Register", model);
            }

            var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            HttpContext.Session.SetObjectAsJson("PendingRegistration", new
PendingRegistration
            {
                User = model,
                Token = token
            });

            await _emailSender.SendEmailAsync(
                model.Email,
                "Your verification token",
                $"Your verification token is: <strong>{token}</strong>"
            );

            return View("RegisterConfirmation");
        }



        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmailToken(string token)
        {
            var pending =
HttpContext.Session.GetObjectFromJson<PendingRegistration>("PendingRegistration");
            if (pending == null)
            {
                ModelState.AddModelError("", "No pending registration found.");
                return View("VerifyEmail");
            }


            if (pending.Token != token)
            {
                ModelState.AddModelError("", "Invalid token or email.");
                return View("VerifyEmail");
            }

            var userModel = pending.User as RegisterViewModel;
            var appuser = new ApplicationUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
            };

            var result = await _userManager.CreateAsync(appuser,
userModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View("VerifyEmail");
            }

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(appuser, "User");

            HttpContext.Session.Remove("PendingRegistration");

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
                        return RedirectToAction("Index", "Main");
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
    }
}
