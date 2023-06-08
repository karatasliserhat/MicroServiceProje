﻿using Course.Web.Models;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Course.Web.Controllers
{
    public class AuthController : Controller
    {

        private readonly IIdentityService _service;
        public AuthController(IIdentityService service)
        {
            _service = service;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput signInInput)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _service.SignIn(signInInput);

            if (!response.IsSuccessFull)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError("", x);
                });
                return View();
            }
            return RedirectToAction(nameof(Index), "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _service.RevokeRefreshToken();
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }
    }
}
