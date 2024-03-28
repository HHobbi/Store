using Store.Domain.Entities.Common;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Users
{
    public class User: BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public bool IsActive { get; set; }

        public virtual List<UserInRole> UserInRoles { get; set; }

        public virtual ICollection<Order> Orders { get; set; }


    }
}
