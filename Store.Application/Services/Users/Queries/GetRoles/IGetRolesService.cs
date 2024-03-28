using Store.Common.Dto;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Store.Application.Services.Users.Queries.GetRoles.GetRolesService;

namespace Store.Application.Services.Users.Queries.GetRoles
{
    public interface IGetRolesService
    {
        public ResultDto<List<RolesDto>> Execute();
    }
}
