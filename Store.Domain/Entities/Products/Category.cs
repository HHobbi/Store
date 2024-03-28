using Store.Domain.Entities.Common;
using System.Collections.Generic;

namespace Store.Domain.Entities.Products
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public virtual Category ParentCategory { get; set; }
        public long? ParentCategoryId { get; set; }

        public virtual List<Category> SubCategories { get; set; }

    }
}