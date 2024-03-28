using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Commands.EditeUser
{
    public interface IEditUserService
    {
        public ResultDto Execute(RequestEditeUserDto request);

       
    }

    public class EditUserService : IEditUserService
    {

        private readonly IDatabaseContext _db;
        public EditUserService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto Execute(RequestEditeUserDto request)
        {
            var user = _db.Users.Find(request.UserId);
            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت  نشد"
                };
            }
            else 
            {
                user.FullName = request.FullName;
                _db.SaveChanges();
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "کاربر با موفقیت ویرایش شد"
                };
            }
        }
    }

    public class RequestEditeUserDto 
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
    }
}
