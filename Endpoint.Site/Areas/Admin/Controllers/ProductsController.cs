using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Application.Interfaces.FacadPatterns;
using System.Collections.Generic;
using static Store.Application.Services.Products.Commands.AddNewProduct.AddNewProductService;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductFacad _productFacad;
        public ProductsController(IProductFacad productFacad)
        {
            _productFacad=productFacad;
        }
        [HttpGet]
        public IActionResult AddNewProduct() 
        {
            ViewBag.Categories = new SelectList(_productFacad.GetAllCategoriesService.Execute().Data,"Id","Name");
                return View() ;
        }

        [HttpGet]
        public IActionResult AddNewProduct(RequestAddNewProductDto request, List<AddNewProduct_Features> Features)
        {
            List<IFormFile> images = new List<IFormFile>();
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                images.Add(file);
            }
            request.Images = images;
            request.Features = Features;
            return Json(_productFacad.AddNewProductService.Execute(request));
        }
        [HttpGet]
        public IActionResult DeleteProduct(long Id)
        {
            return Json(_productFacad.DeleteProductService.Execute(Id));
        }
        public IActionResult Index(int Page=1,int Pagesize=20)
        {
            return View(_productFacad.GetProductForAdminService.Execute(Page,Pagesize).Data);
        }

        public IActionResult Detail(long Id) 
        {
            return View(_productFacad.GetProductDetailForAdminService.Execute(Id).Data);
        }
    }
}
