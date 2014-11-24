/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public abstract class BinaryExpression : IExpression
	{
		protected IExpression _left;

		protected IExpression _right;

		public BinaryExpression(IExpression left, IExpression right)
		{
			this._left = left;
			this._right = right;
		}

		public virtual IExpression Left()
		{
			return _left;
		}

		public virtual IExpression Right()
		{
			return _right;
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
			Db4objects.Db4o.NativeQueries.Expr.BinaryExpression casted = (Db4objects.Db4o.NativeQueries.Expr.BinaryExpression
				)other;
			return _left.Equals(casted._left) && (_right.Equals(casted._right)) || _left.Equals
				(casted._right) && (_right.Equals(casted._left));
		}

		public override int GetHashCode()
		{
			return _left.GetHashCode() + _right.GetHashCode();
		}

		public abstract void Accept(IExpressionVisitor arg1);
	}
}
