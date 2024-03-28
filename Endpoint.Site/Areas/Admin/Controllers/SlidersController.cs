
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Common.Commands.AddNewSlider;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IAddNewSliderService _addNewSliderservice;
        public SlidersController(IAddNewSliderService addNewSliderService)
        {
            _addNewSliderservice = addNewSliderService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(IFormFile file, string link)
        {
            _addNewSliderservice.Execute(file, link);
            return View();
        }

        
    }
}
