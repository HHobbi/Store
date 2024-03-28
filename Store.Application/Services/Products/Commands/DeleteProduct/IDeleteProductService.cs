using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.DeleteProduct
{
    public interface IDeleteProductService
    {
        ResultDto Execute(long Id);
    }

    public class DeleteProductService : IDeleteProductService
    {
        private readonly IDatabaseContext _db;
        public DeleteProductService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto Execute(long Id)
        {

            var product = _db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) 
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "عملیات نا موفق: محصول مورد نظر یافت نشد"
                };
            }
            product.IsRemoved = true;
            _db.SaveChanges();
               
            return new ResultDto
            {
                IsSuccess = true,
                Message = "با موفقیت انجام شد"
            };
        }
    }

}
