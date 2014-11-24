/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public class AndExpression : BinaryExpression
	{
		public AndExpression(IExpression left, IExpression right) : base(left, right)
		{
		}

		public override string ToString()
		{
			return "(" + _left + ")&&(" + _right + ")";
		}

		public override void Accept(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
