using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Store.Application.Services.Users.Queries.GetRoles
{
    public class GetRolesService : IGetRolesService
    {
        private readonly IDatabaseContext _db;

        public GetRolesService(IDatabaseContext db)
        {
            _db=db;

        }
        public ResultDto<List<RolesDto>> Execute()
        {
            var roles = _db.Roles.Select(p => new RolesDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            return new ResultDto<List<RolesDto>>()
            {
                Data=roles,
                IsSuccess=true,
                Message=""
            };
        }
    }
}
