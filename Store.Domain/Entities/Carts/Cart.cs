﻿using Store.Domain.Entities.Common;
using Store.Domain.Entities.Products;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Carts
{
    public class Cart: BaseEntity
    {
        public virtual User Users { get; set; }
        public long? UserId { get; set; }

        public Guid BrowserId { get; set; }

        public bool IsFinished { get; set;   }

        public ICollection<CartItem> CartItems { get; set; }

    }
    public class CartItem : BaseEntity 
    {
        public int Count { get; set; }
        public int Price { get; set; }

        public virtual Product Product { get; set; }
        public long ProductId { get; set; }

        public virtual Cart Cart { get; set; }
        public long CartId { get; set; }

       
    }
}
