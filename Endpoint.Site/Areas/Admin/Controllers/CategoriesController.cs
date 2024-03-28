using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Products.Commands.AddNewCategory;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Operator")]
    public class CategoriesController : Controller
    {
        private readonly IProductFacad _productFacad;
        public CategoriesController(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }


        public IActionResult Index(long? ParentId) 
        {
            return View(_productFacad.GetCategoriesService.Execute(ParentId).Data);
        }


         [HttpGet]
        public IActionResult AddNewCategory(long? parentId)
        {
            ViewBag.parentId = parentId;
            return View();
        }

        [HttpPost]
        public IActionResult AddNewCategory(long? ParentId, string Name)
        {
            var requestAddNewCategory = new RequestAddNewCategory() { ParentId = ParentId, Name = Name };
            var result = _productFacad.AddNewCategoryService.Execute(requestAddNewCategory);
            return Json(result);
        }
    }
}
