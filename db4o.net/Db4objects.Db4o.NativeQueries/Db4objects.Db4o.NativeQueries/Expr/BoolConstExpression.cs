/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr;

namespace Db4objects.Db4o.NativeQueries.Expr
{
	public class BoolConstExpression : IExpression
	{
		public static readonly Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression True
			 = new Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression(true);

		public static readonly Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression False
			 = new Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression(false);

		private bool _value;

		private BoolConstExpression(bool value)
		{
			this._value = value;
		}

		public virtual bool Value()
		{
			return _value;
		}

		public override string ToString()
		{
			return _value.ToString();
		}

		public static Db4objects.Db4o.NativeQueries.Expr.BoolConstExpression Expr(bool value
			)
		{
			return (value ? True : False);
		}

		public virtual void Accept(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}

		public virtual IExpression Negate()
		{
			return (_value ? False : True);
		}
	}
}
