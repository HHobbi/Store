using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Queries.GetOrdersForAdmin
{
    public interface IGetOrdersForAdminSevice
    {
        ResultDto<List<OrderDto>> Execute(OrderState orderState);
    }

    public class GetOrdersForAdminSevice : IGetOrdersForAdminSevice
    {
        private readonly IDatabaseContext _db;
        public GetOrdersForAdminSevice(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<List<OrderDto>> Execute(OrderState orderState)
        {
            var orders = _db.Orders
                .Include(p => p.OrderDetails)
                .Where(p => p.OrderState == orderState)
                .OrderByDescending(p => p.Id)
                .ToList()
                .Select(p => new OrderDto
                {
                    InsertTime = p.InsertTime,
                    OrderId=p.Id,
                    OrderState= p.OrderState,
                    ProductCount=p.OrderDetails.Count(),
                    RequestId=p.RequestPayId,
                    UserId=p.UserId,

                })
                .ToList();


            return new ResultDto<List<OrderDto>> 
            {
                Data=orders,
                IsSuccess=true
            };
        }

    }

    public class OrderDto
    {
        public long OrderId { get; set; }
        public DateTime InsertTime { get; set; }
        public long RequestId { get; set; }
        public long UserId { get; set; }
        public OrderState OrderState { get; set; }
        public int ProductCount { get; set; }
    }
}
