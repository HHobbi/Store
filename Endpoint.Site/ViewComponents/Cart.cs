using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Contexts;
using Store.Application.Services.Carts;
using Endpoint.Site.Utilities;
using EndPoint.Site.Utilities;

namespace Endpoint.Site.ViewComponents
{
    public class Cart:ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly CookiMangment _cookiMangment;
        public Cart(ICartService cartService, CookiMangment cookisManager)
        {
            _cartService = cartService;
            _cookiMangment = cookisManager;
        }

        public IViewComponentResult Invoke() 
         {
            var userId = ClaimUtility.GetUserId(HttpContext.User);
            return View("Cart",_cartService.GetMyCart(_cookiMangment.GetBrowserId(HttpContext), userId).Data);
        }
    }
}
