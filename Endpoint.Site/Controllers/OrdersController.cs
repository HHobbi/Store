using EndPoint.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Orders.Queries.GetUserOrders;

namespace Endpoint.Site.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IGetUsersOrdersService _getUsersOrdersService;
        public OrdersController(IGetUsersOrdersService getUsersOrdersService)
        {
            _getUsersOrdersService = getUsersOrdersService;
        }

        public IActionResult Index()
        {
            long userId = ClaimUtility.GetUserId(User).Value;
            return View(_getUsersOrdersService.Execute(userId).Data);
        }
    }
}
