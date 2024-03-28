using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.HomePage.AddHomePageImages;
using Store.Common.Dto;
using Store.Domain.Entities.HomePage;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomePageImagesController : Controller
    {
        private readonly IAddHomePageImagesService _addHomePageImagesService;
        public HomePageImagesController(IAddHomePageImagesService addHomePageImagesService)
        {
            _addHomePageImagesService = addHomePageImagesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(IFormFile File,string Link,ImageLocation imageLocation)
        {
            RequestAddHomePageImagesDto requestAddHomePageImagesDto = new RequestAddHomePageImagesDto()
            {
                File = File,
                Link = Link,
                ImageLocation = imageLocation
            };
            ResultDto resultdto= _addHomePageImagesService.Execute(requestAddHomePageImagesDto);

            return View();
        }
    }
}
