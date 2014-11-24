/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public sealed class IgnoredExpression : IExpression
	{
		public static Db4objects.Db4o.NativeQueries.Expr.IgnoredExpression Instance = new 
			Db4objects.Db4o.NativeQueries.Expr.IgnoredExpression();

		private IgnoredExpression()
		{
		}

		public void Accept(IExpressionVisitor visitor)
		{
		}
	}
}
