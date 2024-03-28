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

namespace Store.Application.Services.Common.Commands.AddNewSlider
{
    public interface IAddNewSliderService
    {
        ResultDto Execute(IFormFile file,string Link);
    }
    public class AddNewSliderService : IAddNewSliderService
    {
        private readonly IDatabaseContext _db;
        private readonly IHostingEnvironment _environment;
        public AddNewSliderService(IDatabaseContext db, IHostingEnvironment environment)
        {
            _db = db;
            _environment = environment;

        }

        public ResultDto Execute(IFormFile file,string Link)
        {
            var resultUpload=UploadFile(file);

            Slider slider = new Slider()
            {
                Link = Link,
                Src = resultUpload.FileNameAddress
            };
            _db.Sliders.Add(slider);
            _db.SaveChanges();
            return new ResultDto
            {
                IsSuccess=true,
                Message="ok"
            };
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
                    FileNameAddress = folder + fileName,
                    Status = true
                };

            }
            return null;
        }
    }
}
