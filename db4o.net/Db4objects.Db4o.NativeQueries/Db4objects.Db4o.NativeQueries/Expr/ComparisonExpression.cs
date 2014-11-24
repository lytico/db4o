/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Expr.Cmp;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public class ComparisonExpression : IExpression
	{
		private FieldValue _left;

		private IComparisonOperand _right;

		private ComparisonOperator _op;

		public ComparisonExpression(FieldValue left, IComparisonOperand right, ComparisonOperator
			 op)
		{
			if (left == null || right == null || op == null)
			{
				throw new ArgumentNullException();
			}
			this._left = left;
			this._right = right;
			this._op = op;
		}

		public virtual FieldValue Left()
		{
			return _left;
		}

		public virtual IComparisonOperand Right()
		{
			return _right;
		}

		public virtual ComparisonOperator Op()
		{
			return _op;
		}

		public override string ToString()
		{
			return _left + " " + _op + " " + _right;
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
			Db4objects.Db4o.NativeQueries.Expr.ComparisonExpression casted = (Db4objects.Db4o.NativeQueries.Expr.ComparisonExpression
				)other;
			return _left.Equals(casted._left) && _right.Equals(casted._right) && _op.Equals(casted
				._op);
		}

		public override int GetHashCode()
		{
			return (_left.GetHashCode() * 29 + _right.GetHashCode()) * 29 + _op.GetHashCode();
		}

		public virtual void Accept(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
