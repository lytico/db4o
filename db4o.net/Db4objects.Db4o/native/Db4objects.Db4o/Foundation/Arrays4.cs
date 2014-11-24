using System.Linq;

namespace Db4objects.Db4o.Foundation
{
	partial class Arrays4
	{
		public static string ToString(object[] array)
		{
			const string ItemSeparator = ", ";
			var result = array.Aggregate(
								"[", 
								(acc, curr) => acc + curr + ItemSeparator);
			
			
			
			var extraItemSeparatorIndex = result.LastIndexOf(ItemSeparator);
			if (extraItemSeparatorIndex > 0)
			{
				result = result.Remove(extraItemSeparatorIndex, ItemSeparator.Length);
			}
			return result + "]";
		}
	}
}
