using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;


namespace Store.Application.Services.Users.Commands.RemoveUser
{
    public class RemoveUserService : IRemoveUserService
    {
        private readonly IDatabaseContext _db;

        public RemoveUserService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto Execute(long UserId)
        {
            var user = _db.Users.Find(UserId);
            if (user == null)
            {

                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربری یافت نشد"
                };
            }
            else
            {
                user.IsRemoved = true;
                user.Removetime=DateTime.Now;
                _db.SaveChanges();
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "کاربر با موفقیت حذف شد"
                };
            }
        }
    }
}
