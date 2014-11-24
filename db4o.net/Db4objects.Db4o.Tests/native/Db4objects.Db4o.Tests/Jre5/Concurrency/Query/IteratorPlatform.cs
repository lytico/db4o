using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Jre5.Concurrency.Query
{
	class IteratorPlatform
	{
		internal static object Next(System.Collections.IEnumerator iterator)
		{
			return Iterators.Next(iterator);
		}
	}
}
