/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public interface IExpression : IExpressionPart
	{
		/// <param name="visitor">
		/// must implement the visitor interface required
		/// by the concrete Expression implementation.
		/// </param>
		void Accept(IExpressionVisitor visitor);
	}
}
