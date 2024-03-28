using Store.Application.Services.Users.Commands.UserLogin;
using Endpoint.Site.Models.ViewModels.AuthenticationViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Users.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.UserLogin;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Endpoint.Site.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IRegisterUserService _registerUSerService;
        private readonly IUserLoginService _userLoginService;

        public AuthenticationController(IRegisterUserService registerUSerService, IUserLoginService userLoginService)
        {
            _registerUSerService = registerUSerService;
            _userLoginService = userLoginService;
        }


        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Signup(SignupViewModel request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.RePassword))
            {
                return Json(new ResultDto { IsSuccess = false, Message = "لطفا فیلد ها را خالی نگذارید" });
            }
            if (User.Identity.IsAuthenticated == true) 
            {
                return Json(new ResultDto { IsSuccess = false, Message = "نمیتوانید ثبت نام مجدد انجام دهید" });

            }
            if (request.Password != request.RePassword)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "رمز عبور و تکرار آن برابر نیست" });

            }
            if (request.Password.Length<8)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "رمز عبور حد اقل باید 8 کاراکتر باشد" });

            }
            string emailRegix = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";
            var match = Regex.Match(request.Email, emailRegix, RegexOptions.IgnoreCase);
            if (!match.Success) 
            {
                return Json(new ResultDto { IsSuccess = false, Message = "ایمیل را به درستی وارد نمایید" });

            }
            //for register Customer
            var signupResult = _registerUSerService.Execute(new RequestRegisterUserDto
            {
                Email = request.Email,
                FullName = request.FullName,
                Password = request.Password,
                RePassword = request.RePassword,
                Roles = new List<RolesInRegisterUserDto>()
                {
                    new RolesInRegisterUserDto()
                    {
                        Id=3
                        //3  is customer role
                    }

                }
            }) ;

            if (signupResult.IsSuccess) 
            {

                //set Login Properties
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,signupResult.Data.UserId.ToString()),
                    new Claim(ClaimTypes.Email,request.Email),
                    new Claim(ClaimTypes.Name,request.FullName),
                    new Claim(ClaimTypes.Role,"Customer"),

                };

                //کاربر را لاگین میکند
                var identity=new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme/*ساختار  بر اساس کوکی ست میشود*/);
                var principle = new ClaimsPrincipal(identity);
                //set Remember Me -----> by IsPersistent
                var properties = new AuthenticationProperties() { IsPersistent = true };
                HttpContext.SignInAsync(principle,properties);

                

            }
            return Json(signupResult);
        }


        [HttpGet]
        public IActionResult Signin(string ReturnUrl= "/")
        {
            ViewBag.url = ReturnUrl;
            return View();
        }
        public IActionResult Signin(string Email,string Password,string url="/"  ) 
        {
            var signupResult = _userLoginService.Execute(Email, Password);
            if (signupResult.IsSuccess) 
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,signupResult.Data.UserId.ToString()),
                    new Claim(ClaimTypes.Email,Email),
                    new Claim(ClaimTypes.Name,signupResult.Data.Name),

                };
                foreach (var item in signupResult.Data.Roles) 
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));

                }

                //for Login
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principle = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties() { IsPersistent = true,ExpiresUtc=DateTime.Now.AddDays(5) };
                HttpContext.SignInAsync(principle, properties);
                    

            }
            return Json(signupResult);
        }



        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            
            return RedirectToAction("Index","Home");
        }
    }
}
