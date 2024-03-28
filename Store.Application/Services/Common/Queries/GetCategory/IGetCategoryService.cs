using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Common.Queries.GetCategory
{
    public interface IGetCategoryService
    {
        ResultDto<List<CategoryDto>> Execute();
    }
    public class GetCategoryService : IGetCategoryService 
    {
        private readonly IDatabaseContext _db;
        public GetCategoryService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<List<CategoryDto>> Execute()
        {
            var category = _db.Categories.Where(p => p.ParentCategoryId == null)
                .ToList()
                .Select(p => new CategoryDto
                {
                    CatId = p.Id,
                    CategoryName = p.Name,

                }).ToList();
            return new ResultDto<List<CategoryDto>> 
            {
                Data=category,
                IsSuccess=true
            };
                

        }
    }
    public class CategoryDto
    {
        public long CatId { get; set; }
        public string CategoryName { get; set; }
    }
}
