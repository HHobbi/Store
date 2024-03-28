using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductDetailForSite
{
    public interface IGetProductDetailForSiteService
    {
        ResultDto<ProductDetailForsiteDto> Execute(long Id);
    }

    public class GetProductDetailForSiteService : IGetProductDetailForSiteService
    {
        private readonly IDatabaseContext _db;
        public GetProductDetailForSiteService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<ProductDetailForsiteDto> Execute(long Id)
        {
            var product = _db.Products.
                Include(p => p.Category)
                .ThenInclude(p => p.ParentCategory)
                .Include(p=>p.ProductImages)
                .Include(p=>p.ProductFeatures)
                .Where(p => p.Id == Id).FirstOrDefault();   
            if (product == null) 
            {
                throw new Exception("Product Not Found...");
            }
            product.ViewCount++;
            _db.SaveChanges();
            return new ResultDto<ProductDetailForsiteDto>
            {
                Data = new ProductDetailForsiteDto
                {
                    Id = product.Id,
                    Title = product.Name,
                    Brand = product.Brand,
                    Category = $"{product.Category.ParentCategory.Name} - {product.Category.Name}",
                    Description = product.Description,
                    Price = product.Price,
                    Images = product.ProductImages.Select(p => p.Src).ToList(),
                    Features = product.ProductFeatures.Select(p=>new ProductDetailForSite_FeaturesDto 
                    {
                        DisplayName = p.DisplayName,
                        Value=p.Value
                    }).ToList()
                },
                IsSuccess=true,
                Message=""
            };
        }
    }
    public class ProductDetailForsiteDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public List<string> Images { get; set; }
        public List<ProductDetailForSite_FeaturesDto> Features { get; set; }
    }
    public class ProductDetailForSite_FeaturesDto
    {
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }
}
