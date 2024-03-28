using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Store.Application.Services.Products.Commands.AddNewProduct.AddNewProductService;

namespace Store.Application.Services.Products.Commands.AddNewProduct
{
    public interface IAddNewProductservice
    {
        public ResultDto Execute(RequestAddNewProductDto request);
    }


    public class AddNewProductService : IAddNewProductservice
    {

        private readonly IDatabaseContext _db;
        private readonly IHostingEnvironment _environment;
        public AddNewProductService(IDatabaseContext db,IHostingEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public ResultDto Execute(RequestAddNewProductDto request)
        {
            var category = _db.Categories.Find(request.CategoryId);

            Product product = new Product()
            {
                Name=request.Name,
                Description=request.Description,
                Brand=request.Brand,
                Price=request.Price,
                Inventory=request.Inventory,
                Category=category,
                Displayed=request.Displayed
            };
            _db.Products.Add(product);

            List<ProductImages> productImages = new List<ProductImages>();
            foreach (var item in request.Images) 
            {
                var uploadedResult = UploadFile(item);
                productImages.Add(new ProductImages
                {
                    Product = product,
                    Src = uploadedResult.FileNameAddress
                }) ;
            }
            _db.ProductImages.AddRange(productImages);

            List<ProductFeature> productFeatures  = new List<ProductFeature>();
            foreach (var item in request.Features)
            {
                productFeatures.Add(new ProductFeature
                {
                    DisplayName=item.DisplayName,
                    Value=item.Value,
                    Product=product
                });

            }
            _db.ProductFeatures.AddRange(productFeatures);

            _db.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "محصول با موفقیت به فروشگاه اضافه شد"
            };  

        }

        public ResultDto Execute(long CategoryID, string Name)
        {
            throw new NotImplementedException();
        }

        public class RequestAddNewProductDto
        {
            public string Name { get; set; }
            public string Brand { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public int Inventory { get; set; }
            public long CategoryId { get; set; }
            public bool Displayed { get; set; }
            public List<IFormFile> Images { get; set; }
            public List<AddNewProduct_Features> Features { get; set; }

        }

        public class AddNewProduct_Features 
        {
            public string DisplayName { get; set; }
            public string Value { get; set; }
        }

        public class UploadDto 
        {
            public bool Status { get; set; }
            public string FileNameAddress { get; set; }
        }
        private UploadDto UploadFile(IFormFile file) 
        {
            if (file != null) 
            {
                string folder = $@"Images\ProductImages\";
                var uploadRootFolder = Path.Combine(_environment.WebRootPath, folder);
                if (!Directory.Exists(uploadRootFolder)) 
                {
                    Directory.CreateDirectory(uploadRootFolder);
                }
                if (file == null || file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = ""
                    };
                }
                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(uploadRootFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) 
                {
                    file.CopyTo(fileStream);
                }
                return new UploadDto()
                {
                    FileNameAddress=folder+fileName,
                    Status=true
                };

            }
            return null; 
        }
     }
}
