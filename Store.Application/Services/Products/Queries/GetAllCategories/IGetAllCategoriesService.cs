using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetAllCategories
{
    public interface IGetAllCategoriesService
    {
        ResultDto<List<AllCategoryDto>> Execute();
    }

    public class GetAllCategoriesService : IGetAllCategoriesService
    {

        private readonly IDatabaseContext _db;
        public GetAllCategoriesService(IDatabaseContext db)
        {
            _db = db;
        }
        public ResultDto<List<AllCategoryDto>> Execute()
        {
            var categories = _db.Categories
                .Include(p => p.ParentCategory)
                .Where(p => p.ParentCategoryId != null)
                .ToList()
                .Select(p=>new AllCategoryDto 
                {
                    Id=p.Id,
                    Name=$"{p.ParentCategory.Name}-{p.Name}"
                }).ToList();
            return new ResultDto<List<AllCategoryDto>>
            {
                Data = categories,
                IsSuccess = true,
                Message = ""
            };
                
        }
    }
    public class AllCategoryDto 
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
