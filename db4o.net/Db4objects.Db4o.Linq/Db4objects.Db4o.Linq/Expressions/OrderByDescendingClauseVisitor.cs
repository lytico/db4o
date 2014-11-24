/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Linq.Expressions;
using System.Reflection;
using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.Linq.Caching;
using Db4objects.Db4o.Linq.Internals;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq.Expressions
{
	internal class OrderByDescendingClauseVisitor : OrderByClauseVisitorBase
	{
		private static ICache4<Expression, IQueryBuilderRecord> _cache = ExpressionCacheFactory.NewInstance(10);

		protected override ICache4<Expression, IQueryBuilderRecord> GetCachingStrategy()
		{
			return _cache;
		}

		protected override void ApplyDirection(IQuery query)
		{
			query.OrderDescending();
		}
	}
}
