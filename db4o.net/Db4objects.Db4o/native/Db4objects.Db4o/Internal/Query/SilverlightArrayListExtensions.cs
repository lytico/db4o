using System.Collections;

namespace Db4objects.Db4o.Internal.Query
{
#if SILVERLIGHT
	public static class SilverlightArrayListExtensions
	{
		public static void Sort(this ArrayList self, IComparer comparer)
		{
			self.Sort(comparer.Compare);
		}
	}
#endif
}
