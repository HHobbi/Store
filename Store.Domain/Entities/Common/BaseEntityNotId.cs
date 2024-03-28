using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Common
{
    public abstract class BaseEntityNotId
    {
        public DateTime InsertTime { get; set; } = DateTime.Now;

        public DateTime? UpdateTime { get; set; }
        public bool IsRemoved { get; set; } = false;

        public DateTime? Removetime { get; set; }
    }
}
