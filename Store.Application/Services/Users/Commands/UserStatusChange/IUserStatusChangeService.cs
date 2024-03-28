using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Commands
{
    public interface IUserStatusChangeService
    {
       public ResultDto Execute(long UserId);
    }
    
    public class UserStatusChangeService : IUserStatusChangeService
    {
        private readonly IDatabaseContext _db;
        public UserStatusChangeService(IDatabaseContext db )
        {
            _db=db;
        }
        public ResultDto Execute(long UserId)
        {
            var user = _db.Users.Find(UserId);
            if (user == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "کاربری یافت نشد"
                };
            }
            else 
            {
                user.IsActive = !user.IsActive;
                _db.SaveChanges();
                string userstatus = user.IsActive == true ? "فعال" : "غیرفعال";
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "کاربر با موفقیت " + userstatus + "شد" 
                };
            }
        }
    }
}
