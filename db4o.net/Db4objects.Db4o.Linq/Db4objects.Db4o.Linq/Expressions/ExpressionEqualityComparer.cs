/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Db4objects.Db4o.Linq.Expressions
{
	public class ExpressionEqualityComparer : IEqualityComparer<Expression>
	{
		public static ExpressionEqualityComparer Instance = new ExpressionEqualityComparer();

		public bool Equals(Expression a, Expression b)
		{
			return new ExpressionComparison(a, b).AreEqual;
		}

		public int GetHashCode(Expression expression)
		{
			return new HashCodeCalculation(expression).HashCode;
		}
	}
}
