using Store.Application.Interfaces.Contexts;
using Store.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Queries.GetUsers
{

    public class GetUsersService : IGetUsersService
    {

        private readonly IDatabaseContext _db;
        public GetUsersService(IDatabaseContext db)
        {
            _db = db;
        }
        public ResultGetUserDto Execute(RequestGetUserDto request)
        {
            var users = _db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Searchkey))
            {
                users = users.Where(p=>p.FullName.Contains(request.Searchkey) || p.Email.Contains(request.Searchkey));
            }
            var rowsCount = 0;
            var UserList= users.ToPaged(request.Page, 20, out rowsCount).Select(p => new GetUserDto
            {
                Id = p.Id,
                Email=p.Email,
                FullName=p.FullName,
                IsActive = p.IsActive
                
            }).ToList();

            return new ResultGetUserDto
            {
                Rows = rowsCount,
                Users = UserList
            };

        }


    }
}
