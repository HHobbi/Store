using Endpoint.Site.Utilities;
using EndPoint.Site.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Carts;

namespace Endpoint.Site.Controllers
{
    public class CartController: Controller
    {

        private readonly ICartService _cartService;
        private readonly CookiMangment _cookieManager;
        public CartController(ICartService cartService, CookiMangment cookieManager)
        {
            _cartService=cartService;
            _cookieManager = cookieManager;
        }
        public IActionResult Index()
        {
            var userId = ClaimUtility.GetUserId(User);

            var resultGetLst = _cartService.GetMyCart(_cookieManager.GetBrowserId(HttpContext), userId);

            return View(resultGetLst.Data);
        }
        public IActionResult AddToCart(long ProductId)
        {
            var resultAdd=_cartService.AddToCart(ProductId, _cookieManager.GetBrowserId(HttpContext));
            return RedirectToAction("Index");
        }

        public IActionResult Add(long CartItemId)
        {
            _cartService.Add(CartItemId);
            return RedirectToAction("Index");
        }

        public IActionResult LowOff(long CartItemId)
        {
            _cartService.LowOff(CartItemId);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(long cartItemID) 
        {
            var browserId = _cookieManager.GetBrowserId(HttpContext);
            _cartService.RemoveFromCart(cartItemID, browserId);
            return RedirectToAction("Index");
        }
    }
}
