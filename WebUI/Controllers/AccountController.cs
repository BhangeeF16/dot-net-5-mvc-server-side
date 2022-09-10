using Common.ViewModels.ResetPassword;
using Common.ViewModels.UserAccount;
using Domain.Entities.IdentityModule;
using Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SmartAdmin.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly IEmailService _emailService;
        private readonly UserManager<UserInfo> _userManager;
        private readonly IDataProtector _dataProtector;
        public AccountController(IAccountService accountService, SignInManager<UserInfo> signInManager, IEmailService emailService, UserManager<UserInfo> userManager, IDataProtectionProvider dataProtectionProvider)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _dataProtector = dataProtectionProvider.CreateProtector(new DataProtectionTokenProviderOptions().Name);
        }
        public IActionResult Confirmation() => View();
        public IActionResult Error() => View();
        public IActionResult Error404() => View();
        public IActionResult ErrorAnnounced() => View();
        public IActionResult Forgot() => View();
        [HttpPost]
        public async Task<IActionResult> Forgot(string Email)
        {
            try
            {
                return Json(await _emailService.ForgotPassword(Email));
            }
            catch (Exception ex)
            {
                return Json(false);
            }

        }
        public IActionResult Locked() => View();
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserSignupVM userVM)
        {
            var authUser = await _accountService.Authenticate(userVM.Email, userVM.Password);
            if (authUser != null)
            {
                try
                {
                    var user = new UserInfo
                    {
                        UserName = authUser.Email,
                        FirstName = authUser.FirstName,
                        LastName = authUser.LastName,
                    };
                    var additionalClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, authUser.FirstName + " " + authUser.LastName)
                    };
                    foreach (var role in authUser.Roles)
                    {
                        additionalClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    additionalClaims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(authUser)));
                    await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, additionalClaims);

                    return RedirectToAction("index", "home");
                }
                catch (Exception e)
                {
                    ViewBag.WrongPassword = true;
                    return View("Login");
                }
            }
            else
            {
                ViewBag.WrongPassword = true;
                return View("Login");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult LoginAlt() => View();
        public IActionResult Register() => View();

        private static byte[] DecodeUrlBase64(string s)
        {
            s = s.Replace(' ','+').Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            return Convert.FromBase64String(s);
        }
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> ResetPassword(string token)
        {
            try
            {
                var resetTokenArray = DecodeUrlBase64(HttpUtility.UrlDecode(token));
                //var resetTokenArray = Convert.FromBase64String(plainToken);
                var unprotectedResetTokenArray = _dataProtector.Unprotect(resetTokenArray);
                using (var ms = new MemoryStream(unprotectedResetTokenArray))
                {
                    using (var reader = new BinaryReader(ms))
                    {
                        // Read off the creation UTC timestamp
                        reader.ReadInt64();
                        // Then you can read the userId!
                        var userId = reader.ReadString();
                        var userInfo = _userManager.FindByIdAsync(userId).Result;
                        if (userInfo != null && userInfo.IsActive)
                        {
                            var isValid = _userManager.VerifyUserTokenAsync(userInfo, TokenOptions.DefaultProvider, "ResetPassword", token.Replace(' ', '+')).Result;
                            if (isValid)
                            {
                                var model = new ResetPasswordVM { Token = token, Email = userInfo.Email };
                                return View(model);
                            }
                            else
                            {
                                ViewBag.Message = "Invalid Token";
                                return View("Error");
                            }
                        }
                        else
                        {
                            ViewBag.Message = "User Not Found";
                            return View("Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Something went wrong";
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var result = await _accountService.ResetPassword(resetPasswordModel);
            if (result.Success != false)
            {
                ViewBag.SendEmail = true;
            }
            else
            {
                ViewBag.SendEmail = false;
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVm)
        {
            try
            {
                var result = await _accountService.ChangePassword(changePasswordVm);
                return Json(result);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }

    }
}
