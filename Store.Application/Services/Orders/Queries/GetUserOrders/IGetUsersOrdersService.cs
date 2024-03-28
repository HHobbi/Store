using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Queries.GetUserOrders
{
    public interface IGetUsersOrdersService

    {
        //کاربر در این مرحله باید لاگین کرده باشد
         ResultDto<List<GetUserOrdersDto>> Execute(long UserId); 
    }

    public class GetUsersOrdersService : IGetUsersOrdersService 
    {
        private readonly IDatabaseContext _db;
        public GetUsersOrdersService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<List<GetUserOrdersDto>> Execute(long UserId)
        {
            var orders = _db.Orders.Include(p => p.OrderDetails)
                .ThenInclude(p => p.Product)
                .Where(p => p.UserId == UserId)
                .OrderByDescending(p => p.Id).ToList().Select(p => new GetUserOrdersDto
                {
                    OrderId = p.Id,
                    OrderState = p.OrderState,
                    RequestPayId = p.RequestPayId,
                    orderDetails = p.OrderDetails.Select(o => new OrderDetailDto
                    {
                        Count = o.Count,
                        OrderDetailId = o.Id,
                        Price = o.Price,
                        ProductId = o.ProductId,
                        ProductName = o.Product.Name,

                    }).ToList()

                }).ToList();

            return new ResultDto<List<GetUserOrdersDto>>
            {
                Data=orders,
                IsSuccess=true,
                Message=""
            };

        }

    }

    public class GetUserOrdersDto 
    {
        public long OrderId { get; set; }
        public OrderState OrderState { get; set; }
        public long RequestPayId { get; set; }

        public List<OrderDetailDto> orderDetails { get; set; }

    }

    public class OrderDetailDto 
    {
        public long OrderDetailId  { get; set; }
        public long ProductId  { get; set; }
        public string ProductName  { get; set; }
        public int Price  { get; set; }
        public int Count { get; set; }
    }

}
