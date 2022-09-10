using Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartAdmin.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICurrentUserService _currentUserService;
        public HomeController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        public async Task<IActionResult> Index()
        {
           return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordTemplate()
        {
            return View("~/Views/EmailTemplates/ResetPassword.cshtml");
        }
        [AllowAnonymous]
        public IActionResult ForgotPasswordTemplate()
        {
            return View("~/Views/EmailTemplates/ForgotPassword.cshtml");
        }

        public IActionResult Profile() => View();
        public IActionResult Checkout() => View();
    }
}
