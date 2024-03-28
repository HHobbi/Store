using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Common.Queries.GetMenuItem
{
    public interface IGetMenuItemService
    {
        ResultDto<List<MenuItemDto>> Execute();
    }
    public class GetMenuItemService : IGetMenuItemService 
    {
        private readonly IDatabaseContext _db;
        public GetMenuItemService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<List<MenuItemDto>> Execute()
        {
            var category = _db.Categories
                  .Include(p => p.SubCategories)
                  .Where(p=>p.ParentCategoryId ==null)
                  .ToList()
                  .Select(p => new MenuItemDto
                  {
                      CatId = p.Id,
                      Name = p.Name,
                      Child = p.SubCategories.ToList().Select(child => new MenuItemDto { 
                      
                          CatId=child.Id,
                          Name=child.Name,

                      }).ToList()
                  }).ToList();

            return new ResultDto<List<MenuItemDto>>
            {
                Data= category,
                IsSuccess=true,
                Message=""
            };
        }

       
    }

    public class MenuItemDto 
    {
        public long CatId { get; set; }
        public string Name { get; set; }    
        public List<MenuItemDto> Child { get; set; }
    }
}
