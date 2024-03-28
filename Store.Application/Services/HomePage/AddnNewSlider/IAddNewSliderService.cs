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

namespace Store.Application.Services.HomePage.AddnNewSlider
{
    public interface IAddNewSliderService
    {
        ResultDto Execute(IFormFile file,string link);
    }

    public class AddNewSliderService : IAddNewSliderService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IDatabaseContext _context;


        public AddNewSliderService(IHostingEnvironment environment, IDatabaseContext context)
        {
            _environment = environment;
            _context = context;
        }

        public ResultDto Execute(IFormFile file,string link)
        {
            var resultUpload = UploadFile(file);
            Slider slider = new Slider()
            {
                Link=link,
                Src=resultUpload.FileNameAddress
            };
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true
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

}
