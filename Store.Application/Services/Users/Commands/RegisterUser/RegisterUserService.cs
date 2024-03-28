using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using Store.Domain.Entities.Users;
using System.Collections.Generic;

namespace Store.Application.Services.Users.Commands.RegisterUser
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDatabaseContext _db;

        public RegisterUserService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0
                        },
                        IsSuccess = false,
                        Message = "پست الکترونیک را وارد نمایید"
                    };
                }


                if (string.IsNullOrWhiteSpace(request.FullName))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0
                        },
                        IsSuccess = false,
                        Message = " نام را وارد نمایید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0
                        },
                        IsSuccess = false,
                        Message = " رمز عبور را وارد نمایید"
                    };
                }

                if (request.RePassword != request.Password)
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0
                        },
                        IsSuccess = false,
                        Message = " رمز عبور و تکرار آن برابر نیست"
                    };
                }
                var passwordHasher = new PasswordHasher();
                var hashedPassword = passwordHasher.HashPassword(request.Password);

                User user = new User()
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Password = hashedPassword,
                    IsActive=true

                };
                List<UserInRole> userInRoles = new List<UserInRole>();
                foreach (var item in request.Roles)
                {
                    Role role = _db.Roles.Find(item.Id);
                    userInRoles.Add(new UserInRole()
                    {
                        Role = role,
                        RoleId = role.Id,
                        User = user,
                        UserId = user.Id
                    }
                    );

                }
                user.UserInRoles = userInRoles;
                _db.Users.Add(user);
                _db.SaveChanges();
                return new ResultDto<ResultRegisterUserDto>
                {
                    Data = new ResultRegisterUserDto { UserId = user.Id },
                    IsSuccess = true,
                    Message = "ثبت نام کاربر انجام شد"

                };
            }
            catch 
            {
                return new ResultDto<ResultRegisterUserDto>
                {
                    Data = new ResultRegisterUserDto { UserId = 0 },
                    IsSuccess = false,
                    Message = "ثبت نام کاربر انجام نشد"

                };
            }

           
           
        }
    }
}
