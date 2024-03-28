using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductForAdmin
{
    public interface IGetProductForAdminService
    {
        ResultDto<ProductforAdminDto> Execute(int Page,int Pagesize);
    }

    public class GetProductForAdminService:IGetProductForAdminService
    {
        private readonly IDatabaseContext _db;

        public GetProductForAdminService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<ProductforAdminDto> Execute(int Page=1,int Pagesize=20)
        {
            int RowCount = 0;
            var Products = _db.Products
                .Include(p=>p.Category)
                .ToPaged(Page,Pagesize,out RowCount)
                .Select(p=>new ProductForAdminList_Dto
                {
                    Id=p.Id,
                    Name=p.Name,
                    Brand=p.Brand,
                    Category=p.Category.Name,
                    Description=p.Description,
                    Inventory=p.Inventory,
                    Price=p.Price,
                    Displayed=p.Displayed,

                }).ToList();
            return new ResultDto<ProductforAdminDto> 
            {
                Data = new ProductforAdminDto
                {
                    Products=Products,
                    CurrentPage=Page,
                    PageSize=Pagesize,
                    RowCount=RowCount
                },
                IsSuccess=true,
                Message=""
                
            };
        }

       

       
    }
    public class ProductforAdminDto
    {
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public List<ProductForAdminList_Dto> Products { get; set; }


    }
    public class ProductForAdminList_Dto 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public bool Displayed { get; set; }
    }
}
