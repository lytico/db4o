using System.Linq.Expressions;
using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.Linq.Caching;
using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq.Expressions
{
	internal class ExpressionCacheFactory
	{
		internal static ICache4<Expression, IQueryBuilderRecord> NewInstance(int cacheSize)
		{
			return CacheFactory<Expression, IQueryBuilderRecord>.For(CacheFactory.New2QXCache(cacheSize), ExpressionEqualityComparer.Instance);
		}
	}
}
