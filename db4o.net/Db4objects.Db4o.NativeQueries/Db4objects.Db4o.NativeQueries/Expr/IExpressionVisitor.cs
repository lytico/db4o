/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public interface IExpressionVisitor
	{
		void Visit(AndExpression expression);

		void Visit(OrExpression expression);

		void Visit(NotExpression expression);

		void Visit(ComparisonExpression expression);

		void Visit(BoolConstExpression expression);
	}
}
