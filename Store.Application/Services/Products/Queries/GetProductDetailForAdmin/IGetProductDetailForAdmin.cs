using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductDetailForAdmin
{
    public interface IGetProductDetailForAdminService
    {
        ResultDto<ProductDetailForAdminDto> Execute(long Id);
    }
    public class GetProductDetailForAdminService : IGetProductDetailForAdminService
    {
        private readonly IDatabaseContext _db;
        public GetProductDetailForAdminService(IDatabaseContext db)
        {
            _db = db;
        }
        public ResultDto<ProductDetailForAdminDto> Execute(long Id)
        {
            var product = _db.Products
                .Include(p => p.Category)
                .ThenInclude(p => p.ParentCategory)
                .Include(p => p.ProductFeatures)
                .Include(p => p.ProductImages)
                .Where(p => p.Id == Id)
                .FirstOrDefault();
            return new ResultDto<ProductDetailForAdminDto>
            {
                Data = new ProductDetailForAdminDto
                {
                    Brand = product.Brand,
                    Category = GetCategory(product.Category),
                    Description = product.Description,
                    Displayed = product.Displayed,
                    Id = product.Id,
                    Inventory = product.Inventory,
                    Name = product.Name,
                    Price = product.Price,
                    Featuers = product.ProductFeatures.ToList().Select(p => new ProdictDetailFeatuerDto
                    {
                        Id = p.Id,
                        DisplayName = p.DisplayName,
                        Value = p.Value,
                    }).ToList(),
                    Images = product.ProductImages.ToList().Select(p=>new ProdictDetailImagesDto 
                    {
                        Id=p.Id,
                        Src=p.Src
                    }).ToList()

                },
                IsSuccess=true,
                Message=""
            };
                
                
        }
        private string GetCategory(Category category)
        {
            string result = category.ParentCategory != null ? $"{ category.ParentCategory.Name} - ":"";
            return result += category.Name;

        }



    }

    public class ProductDetailForAdminDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public bool Displayed { get; set; }

        public List<ProdictDetailFeatuerDto> Featuers { get; set; }
        public List<ProdictDetailImagesDto> Images { get; set; }

    }

    public class ProdictDetailFeatuerDto
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }


        
    }
    public class ProdictDetailImagesDto
    {
        public long Id { get; set; }
        public string Src { get; set; }
    }
}
