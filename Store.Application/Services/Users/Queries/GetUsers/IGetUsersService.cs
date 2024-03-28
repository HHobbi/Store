using System.Collections.Generic;

namespace Store.Application.Services.Users.Queries.GetUsers
{
    public interface IGetUsersService
    {
        public ResultGetUserDto Execute(RequestGetUserDto requestGetUserDto);
    }
}
