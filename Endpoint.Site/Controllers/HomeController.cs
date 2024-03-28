using Endpoint.Site.Models;
using Endpoint.Site.Models.ViewModels.HomePage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Common.Queries.GetHomePageImages;
using Store.Application.Services.Common.Queries.GetSlider;
using Store.Application.Services.Products.Queries.GetProductForsite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetSliderService _getSliderService;
        private readonly IGetHomePageImagesService _getHomePageImagesService;
        private readonly IProductFacad _productFacaad;

        public HomeController(ILogger<HomeController> logger,IGetSliderService getSliderService, IGetHomePageImagesService getHomePageImagesService,IProductFacad productFacad)
        {
            _logger = logger;
            _getSliderService = getSliderService; 
            _getHomePageImagesService = getHomePageImagesService;
            _productFacaad = productFacad;
        }

        public IActionResult Index()
        {
            HomePageViewModel homePage = new HomePageViewModel()
            {
                Sliders = _getSliderService.Execute().Data,
                PageImages = _getHomePageImagesService.Execute().Data,
                Camera = _productFacaad.GetProductForSiteService.Execut(Ordering.TheNewst,null,1,6,10010).Data.Products,
                Mobile=_productFacaad.GetProductForSiteService.Execut(Ordering.TheNewst,null,1,6,10013).Data.Products,
                Kitchen_Tools=_productFacaad.GetProductForSiteService.Execut(Ordering.TheNewst,null,1,6,10016).Data.Products,
                Laptop=_productFacaad.GetProductForSiteService.Execut(Ordering.TheNewst,null,1,6,10019).Data.Products




            };
            
            
            return View(homePage);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
