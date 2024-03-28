using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetCategories
{
    public interface IGetCategoriesService
    {
        ResultDto<List<CategoriesDto>> Execute(long? ParentID);
    }

    public class GetCategoriesService : IGetCategoriesService 
    {
        private readonly IDatabaseContext _db;
        public GetCategoriesService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<List<CategoriesDto>> Execute(long? ParentId)
        {
            var categories = _db.Categories
               .Include(p => p.ParentCategory)
               .Include(p => p.SubCategories)
               .Where(p => p.ParentCategoryId == ParentId)
               .ToList()
               .Select(p => new CategoriesDto
               {
                   Id = p.Id,
                   Name = p.Name,
                   Parent = p.ParentCategory != null ? new
                   ParentCategoriesDto
                   {
                       Id = p.ParentCategory.Id,
                       Name = p.ParentCategory.Name,
                   }
                   : null,
                   HasChild = p.SubCategories.Count() > 0 ? true : false,
               }).ToList();
            return new ResultDto<List<CategoriesDto>>
            {
                Data = categories,
                IsSuccess = true,
                Message = "لیست با موفقیت برگشت داده شد"
            };
        }
    }
    public class CategoriesDto 
    {
        public long Id { get; set; }
        public string  Name { get; set; }

        public bool HasChild { get; set; }

        public ParentCategoriesDto Parent { get; set; }
    }

    public class ParentCategoriesDto 
    {
        public long Id { get; set; }
        public string Name  { get; set; }
    }
}
