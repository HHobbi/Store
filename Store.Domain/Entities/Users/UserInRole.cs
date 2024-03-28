using Store.Domain.Entities.Common;

namespace Store.Domain.Entities.Users
{
    public class UserInRole :BaseEntity
    {
        public long Id { get; set; }
        public virtual User User { get; set; }
        public long UserId { get; set; }
        public Role Role { get; set; }
        public long RoleId { get; set;  }


    }
}
