using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
	using System.Threading.Tasks;

namespace Store.Common
{
	public static class Pagination
	{
		public static IEnumerable<T> ToPaged<T>(this IEnumerable<T> source, int Page, int PageSize, out int rowsCount)
		{
			rowsCount = source.Count();
			return source.Skip((Page-1)*PageSize).Take(PageSize);
		}
		 
 	}
}
