using Store.Domain.Entities.Common;
using System.Collections.Generic;

namespace Store.Domain.Entities.Users
{
    public class Role :BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual List<UserInRole> UserInRoles { get; set; }

    }
}
