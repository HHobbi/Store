using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.AddNewCategory
{

    public interface IAddNewCategoryService
    {
        public ResultDto Execute(RequestAddNewCategory request);
    }
    public class AddNewCategoryService : IAddNewCategoryService
    {

        private readonly IDatabaseContext _db;
        public AddNewCategoryService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto Execute(RequestAddNewCategory request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "نام دسته را وارد نمایید"
                };

            }
            Category category=new Category();
            category.Name = request.Name;
            category.ParentCategory = GetParent(request.ParentId);
            _db.Categories.Add(category);
            _db.SaveChanges();
            return new ResultDto
            {
                IsSuccess=true,
                Message="دسته جدید با موفقیت اضافه شد "
            };

        }
        private Category GetParent(long? ParentId)
        {

            return _db.Categories.Find(ParentId);

        }
    }
   

    public class RequestAddNewCategory 
    {
        public long? ParentId { get; set; }
        public string Name { get; set; }
    }

   
}
