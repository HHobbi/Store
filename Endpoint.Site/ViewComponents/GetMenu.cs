using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Common.Queries.GetMenuItem;

namespace Endpoint.Site.ViewComponents
{
    public class GetMenu:ViewComponent
    {
        private readonly IGetMenuItemService _getMenuItemService;
        public GetMenu(IGetMenuItemService getMenuItemService)
        {
            _getMenuItemService = getMenuItemService;
        }

        public IViewComponentResult Invoke() 
        {
            var menuItem = _getMenuItemService.Execute();
            return View(viewName: "GetMenu",menuItem.Data);
        }
    }
}
