using Store.Domain.Entities.Common;
using Store.Domain.Entities.Finance;
using Store.Domain.Entities.Products;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Orders
{
    public class Order:BaseEntity
    {
        public virtual User User { get; set; }
        public long UserId { get; set; }

        public virtual RequestPay RequestPay { get; set; }
        public long RequestPayId { get; set; }

        public OrderState OrderState { get; set; }
        public string  Address { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
    public class OrderDetail: BaseEntity 
    {
        public virtual Order Order { get; set; }
        public long OrdrId { get; set; }

        public virtual Product Product { get; set; }
        public long ProductId { get; set; }

        public int Price { get; set; }
        public int Count { get; set; }
    }

    public enum OrderState
    {
        [Display(Name="درحال پردازش")]
        processing=0,
        [Display(Name ="لغو شده")]
        Canceled=1,
        [Display(Name = "تحویل شده")]
        Delivered = 2
    }
}
