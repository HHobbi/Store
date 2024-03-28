using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductForsite
{
    public interface IGetProductForSiteService
    {
        ResultDto<ResultProductForsiteDto> Execut(Ordering ordering, string SearchKey,int Page, int Pagesize, long? CatId);
    }

    public class GetProductForSiteService : IGetProductForSiteService
    {

        private readonly IDatabaseContext _db;

        public GetProductForSiteService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<ResultProductForsiteDto> Execut(Ordering ordering, string SearchKey, int Page, int Pagesize, long? CatId)
        {
            int totalRow = 0;
            var productsQuery = _db.Products
                .Include(p => p.ProductImages).AsQueryable();

            if (CatId != null) 
            {
                productsQuery=productsQuery.Where(p => p.CategoryId == CatId || p.Category.ParentCategoryId==CatId).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(SearchKey)) 
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(SearchKey) || p.Brand.Contains(SearchKey)).AsQueryable();
            }
            switch (ordering) 
            {
                case Ordering.NotOrder:
                    {
                        productsQuery = productsQuery.OrderByDescending(p => p.Id).AsQueryable();
                        break;
                    }
                case Ordering.MostVisit:
                    {
                        productsQuery = productsQuery.OrderByDescending(p => p.ViewCount).AsQueryable();
                        break;
                    }
                case Ordering.BestSelling:
                    {
                        productsQuery = productsQuery.OrderByDescending(p => p.Id).AsQueryable();
                        break;
                    }
                case Ordering.MostPopular:
                    {
                        productsQuery = productsQuery.OrderByDescending(p => p.Id).AsQueryable();
                        break;
                    }
                case Ordering.TheNewst:
                    {
                        productsQuery = productsQuery.OrderByDescending(p => p.Id).AsQueryable();
                        break;
                    }
                case Ordering.Cheapest:
                    {
                        productsQuery = productsQuery.OrderBy(p => p.Price).AsQueryable();
                        break;
                    }
                case Ordering.TheMostExpensive:
                    {
                        productsQuery = productsQuery.OrderByDescending(p => p.Price).AsQueryable();
                        break;
                    }
            }
                
                var products = productsQuery.ToPaged(Page, Pagesize, out totalRow);
            Random rd = new Random();

            return new ResultDto<ResultProductForsiteDto>
            {

                Data = new ResultProductForsiteDto
                {
                    TotalRow = totalRow,
                    Products = products.Select(p => new ProductForsiteDto
                    {
                        Id = p.Id,
                        Star = rd.Next(1, 5),
                        Title = p.Name,
                        ImageSrc = p.ProductImages.FirstOrDefault().Src,
                        Price=p.Price

                    }).ToList()

                },
                IsSuccess = true,
                Message = ""

            };


        }




    }
    public class ResultProductForsiteDto
    {
        public List<ProductForsiteDto> Products { get; set; }
        public int TotalRow { get; set; }
    }
    public class ProductForsiteDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ImageSrc { get; set; }
        public int Star { get; set; }
        public int Price { get; set; }
    }

    public enum Ordering 
    {
        NotOrder=0,
        MostVisit=1,
        BestSelling=2,
        MostPopular=3,
        TheNewst=4,
        Cheapest=5,
        TheMostExpensive = 6
    }
}
