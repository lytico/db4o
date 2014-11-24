/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public class NotExpression : IExpression
	{
		private IExpression _expr;

		public NotExpression(IExpression expr)
		{
			this._expr = expr;
		}

		public override string ToString()
		{
			return "!(" + _expr + ")";
		}

		public virtual IExpression Expr()
		{
			return _expr;
		}

		public override bool Equals(object other)
		{
			if (this == other)
			{
				return true;
			}
			if (other == null || GetType() != other.GetType())
			{
				return false;
			}
			Db4objects.Db4o.NativeQueries.Expr.NotExpression casted = (Db4objects.Db4o.NativeQueries.Expr.NotExpression
				)other;
			return _expr.Equals(casted._expr);
		}

		public override int GetHashCode()
		{
			return -_expr.GetHashCode();
		}

		public virtual void Accept(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
