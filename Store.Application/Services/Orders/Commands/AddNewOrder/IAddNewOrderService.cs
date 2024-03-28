using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Commands.AddNewOrder
{
    public interface IAddNewOrderService
    {
        ResultDto Execute(RequestAddNewOrederServiceDto request);

    }

    public class AddNewOrderService : IAddNewOrderService
    {
        private readonly IDatabaseContext _db;
        public AddNewOrderService(IDatabaseContext db)
        {
            _db = db;
        }
        public ResultDto Execute(RequestAddNewOrederServiceDto request)
        {

            //ثبت سفارش
            var user = _db.Users.Find(request.UserId);
            var requestPay = _db.RequestPays.Find(request.RequestPayId);
            var cart=_db.Carts.Include(p=>p.CartItems)
                .ThenInclude(p=>p.Product)
                .Where(p=>p.Id==request.CartId).FirstOrDefault();

            requestPay.IsPay = true;
            requestPay.PayDate = DateTime.Now;
            requestPay.RefId = request.RefId;
            requestPay.Authority=request.Authority;
            cart.IsFinished = true;

            Order Order = new Order()
            {
                Address = "",
                OrderState = OrderState.processing,
                RequestPay = requestPay,
                User = user,

            };

            _db.Orders.Add(Order);

            List<OrderDetail> orderDetails=new List<OrderDetail>();
            foreach (var item in cart.CartItems)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    Count = item.Count,
                    Order = Order,
                    Price = item.Product.Price,
                    Product = item.Product,

                };
                orderDetails.Add(orderDetail);
            }
            _db.OrderDetails.AddRange(orderDetails);
            _db.SaveChanges();

            return new ResultDto
            {
                IsSuccess=true,
                Message="",
            };


        }
    }
    public class RequestAddNewOrederServiceDto
    {
        public long CartId { get; set; }
        public long RequestPayId { get; set; }
        public long UserId { get; set; }

        public long RefId { get; set; }
        public string Authority { get; set; }
    }
}
