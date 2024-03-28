using Dto.Payment;
using Endpoint.Site.Utilities;
using EndPoint.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Carts;
using Store.Application.Services.Finance.Commands;
using Store.Application.Services.Finance.Queries.GetRequestPayService;
using Store.Application.Services.Orders.Commands.AddNewOrder;
using System;
using System.Threading.Tasks;
using ZarinPal.Class;

namespace Endpoint.Site.Controllers
{
    [Authorize("Customer")]
    public class PayController : Controller
    {
        private readonly IAddRequestPayService _addRequestPayService;
        private readonly ICartService _cartService;
        private readonly CookiMangment _cookiManger;
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;
        private readonly IGetRequestPayService _getRequestPayService;
        private readonly IAddNewOrderService _addNewOrderService;




        public PayController(IAddRequestPayService addRequestPayService, ICartService cartService,
            CookiMangment cookiMangment, IGetRequestPayService getRequestPayService, IAddNewOrderService addNewOrderService)
        {
            _addRequestPayService = addRequestPayService;
            _cartService = cartService;
            _cookiManger = cookiMangment;
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
            _getRequestPayService = getRequestPayService;
            _addNewOrderService = addNewOrderService;
        }
        public async Task<IActionResult> Index()
        {
            long? userId = ClaimUtility.GetUserId(User);
            var cart = _cartService.GetMyCart(_cookiManger.GetBrowserId(HttpContext), userId).Data;
            if (cart.SumAmount > 0)
            {
                var requestPay = _addRequestPayService.Execute(cart.SumAmount, userId.Value);
                //ارسال به درگاه پرداخت

                var result = await _payment.Request(
                    new DtoRequest()
                    {
                        Mobile = "09121112222",
                        CallbackUrl = $"https://localhost:44316/Pay/Verify?guid={requestPay.Data.guid}",
                        Description = $"پرداخت فاکتور شماره :{requestPay.Data.RequestPayId}",
                        Email = requestPay.Data.Email,
                        Amount = requestPay.Data.Amount,
                        MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
                    },
                    ZarinPal.Class.Payment.Mode.sandbox);
                return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{result.Authority}");


            }
            else
            {
                RedirectToAction("Index", "cart");
            }
            return View();
        }

        public async Task<IActionResult> Verify(Guid guid, string Authority, string Status)
        {
            var requestpay = _getRequestPayService.Execute(guid);
            var verification = await _payment.Verification(new DtoVerification
            {
                Amount = requestpay.Data.Amount,
                MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                Authority = Authority
            }, Payment.Mode.sandbox);


            long? UserId = ClaimUtility.GetUserId(User);
            var cart = _cartService.GetMyCart(_cookiManger.GetBrowserId(HttpContext),UserId);
            if (verification.Status == 100)
            {
                _addNewOrderService.Execute(new RequestAddNewOrederServiceDto 
                {
                    CartId=cart.Data.CartId,
                    UserId=UserId.Value,
                    RequestPayId=requestpay.Data.id,
                });
                //redirect to page after pay
                //example redirect to orders
                return RedirectToAction("Index", "Orders");
            }
            else 
            {

            }
            return View();
        }
    }
}
