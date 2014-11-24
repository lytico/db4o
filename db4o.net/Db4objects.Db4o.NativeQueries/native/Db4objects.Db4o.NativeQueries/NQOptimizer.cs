using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.Instrumentation.Cecil;
using Db4objects.Db4o.Instrumentation.Core;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Optimization;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.NativeQueries
{
	public class NQOptimizer : INQOptimizer
	{	
		private readonly INativeClassFactory _classFactory = new DefaultNativeClassFactory();

		public void Optimize(IQuery q, object predicate, MethodBase filterMethod)
		{
			// TODO: cache predicate expressions here
			QueryExpressionBuilder builder = new QueryExpressionBuilder();
			IExpression expression = builder.FromMethod(filterMethod);
			new SODAQueryBuilder().OptimizeQuery(expression, q, predicate, _classFactory, new CecilReferenceResolver());
		}
	}
}
