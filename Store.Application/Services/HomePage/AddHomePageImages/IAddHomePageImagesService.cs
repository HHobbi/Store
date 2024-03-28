using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.HomePage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Store.Application.Services.Products.Commands.AddNewProduct.AddNewProductService;

namespace Store.Application.Services.HomePage.AddHomePageImages
{
    public interface IAddHomePageImagesService
    {
        ResultDto Execute(RequestAddHomePageImagesDto reques); 
    }
    public class AddHomePageImagesService : IAddHomePageImagesService
    {
        private readonly IDatabaseContext _db;
        private readonly IHostingEnvironment _environment;

        public AddHomePageImagesService(IDatabaseContext db,IHostingEnvironment environment)
        {
            _db = db;
            _environment= environment;
        }
        public ResultDto Execute(RequestAddHomePageImagesDto request)
        {
            var resultUpload = UploadFile(request.File);

            HomePageImages homePageImages = new HomePageImages()
            {
                Src=resultUpload.FileNameAddress,
                Link=request.Link,
                ImageLocation=request.ImageLocation
            };

            _db.HomePageImages.Add(homePageImages);
            _db.SaveChanges();

            return new ResultDto ()
            {
                IsSuccess=true,
                
            };
        }

        private UploadDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"Images\HomePage\Slider\";
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
                    FileNameAddress = folder + fileName,
                    Status = true
                };

            }
            return null;
        }
    }

    public class RequestAddHomePageImagesDto 
    {
        public IFormFile  File { get; set; }
        public string Link { get; set; }
        public ImageLocation ImageLocation { get; set; }
    }
}
