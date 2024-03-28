using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Orders.Queries.GetOrdersForAdmin;
using Store.Domain.Entities.Orders;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Operator")]
    public class OrdersController : Controller
    {

        private readonly IGetOrdersForAdminSevice _getOrdersForAdminSevice;
        public OrdersController(IGetOrdersForAdminSevice getOrdersForAdminSevice)
        {
            _getOrdersForAdminSevice = getOrdersForAdminSevice;
        }

        public IActionResult Index(OrderState orderState)
        {
            return View(_getOrdersForAdminSevice.Execute(orderState).Data);
        }
    }
}
