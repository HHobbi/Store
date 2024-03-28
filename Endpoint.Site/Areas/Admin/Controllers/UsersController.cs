using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Application.Services.Users.Commands;
using Store.Application.Services.Users.Commands.EditeUser;
using Store.Application.Services.Users.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.RemoveUser;
using Store.Application.Services.Users.Queries.GetRoles;
using Store.Application.Services.Users.Queries.GetUsers;
using System.Collections.Generic;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class UsersController : Controller
    {

        private readonly IGetUsersService _getUsersService;
        private readonly IGetRolesService _getRolesService;
        private readonly IRegisterUserService _registerUserService;
        private readonly IRemoveUserService _removeUserService;
        private readonly IUserStatusChangeService _userStatusChangeService;
        private readonly IEditUserService _editeUserService;
        public UsersController(IGetUsersService getUsersService, IGetRolesService getRolesService,
            IRegisterUserService registerUserService, IRemoveUserService removeUserService,
            IUserStatusChangeService userStatusChangeService, IEditUserService editUserService)
        {
            _getUsersService = getUsersService;
            _getRolesService = getRolesService;
            _registerUserService = registerUserService;
            _removeUserService = removeUserService;
            _userStatusChangeService = userStatusChangeService;
            _editeUserService = editUserService;
        }

        public IActionResult Index(string searchkey, int page = 1)
        {
            
            ResultGetUserDto resultGetUserDto = _getUsersService.Execute(new RequestGetUserDto { Searchkey = searchkey, Page = page });
            return View(resultGetUserDto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_getRolesService.Execute().Data, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Email, string FullName, long RoleId, string Password, string RePassword)
        {

            var result = _registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = Email,
                FullName = FullName,
                Password = Password,
                Roles = new List<RolesInRegisterUserDto>()
                {
                    new RolesInRegisterUserDto()
                    {
                        Id=RoleId
                    }
                },
                RePassword = RePassword

            }); ;
            return Json(result);
        }

        [HttpPost]
        public IActionResult Delete(long UserId)
        {
            return Json(_removeUserService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult UserSatusChange(long UserId)
        {
            var result = _userStatusChangeService.Execute(UserId);
            return Json(result);
        }

        [HttpPost]
        public IActionResult Edit(long UserId, string FullName) 
        {
            var result = _editeUserService.Execute(new RequestEditeUserDto
            {
                FullName = FullName,
                UserId = UserId
            }) ;
            return Json(result);
        }
    }
}
