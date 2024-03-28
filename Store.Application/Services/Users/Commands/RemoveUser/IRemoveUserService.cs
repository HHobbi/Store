using Microsoft.EntityFrameworkCore;
using Store.Common.Dto;
using Store.Domain.Entities.Users;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Store.Application.Services.Users.Commands.RemoveUser
{
    public interface IRemoveUserService
    {
        ResultDto Execute(long UserId);
    }
}
