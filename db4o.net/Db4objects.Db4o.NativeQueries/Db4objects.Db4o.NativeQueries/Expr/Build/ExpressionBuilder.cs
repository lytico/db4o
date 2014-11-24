/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Build
{
	public class ExpressionBuilder
	{
		/// <summary>Optimizations: !(Bool)-&gt;(!Bool), !!X-&gt;X, !(X==Bool)-&gt;(X==!Bool)
		/// 	</summary>
		public virtual IExpression Not(IExpression expr)
		{
			if (expr.Equals(BoolConstExpression.True))
			{
				return BoolConstExpression.False;
			}
			if (expr.Equals(BoolConstExpression.False))
			{
				return BoolConstExpression.True;
			}
			if (expr is NotExpression)
			{
				return ((NotExpression)expr).Expr();
			}
			if (expr is ComparisonExpression)
			{
				ComparisonExpression cmpExpr = (ComparisonExpression)expr;
				if (cmpExpr.Right() is ConstValue)
				{
					ConstValue rightConst = (ConstValue)cmpExpr.Right();
					if (rightConst.Value() is bool)
					{
						bool boolVal = (bool)rightConst.Value();
						// new Boolean() instead of Boolean.valueOf() for .NET conversion
						return new ComparisonExpression(cmpExpr.Left(), new ConstValue(!boolVal), cmpExpr
							.Op());
					}
				}
			}
			return new NotExpression(expr);
		}

		/// <summary>Optimizations: f&&X-&gt;f, t&&X-&gt;X, X&&X-&gt;X, X&&!X-&gt;f</summary>
		public virtual IExpression And(IExpression left, IExpression right)
		{
			if (left.Equals(BoolConstExpression.False) || right.Equals(BoolConstExpression.False
				))
			{
				return BoolConstExpression.False;
			}
			if (left.Equals(BoolConstExpression.True))
			{
				return right;
			}
			if (right.Equals(BoolConstExpression.True))
			{
				return left;
			}
			if (left.Equals(right))
			{
				return left;
			}
			if (Negatives(left, right))
			{
				return BoolConstExpression.False;
			}
			return new AndExpression(left, right);
		}

		/// <summary>Optimizations: X||t-&gt;t, f||X-&gt;X, X||X-&gt;X, X||!X-&gt;t</summary>
		public virtual IExpression Or(IExpression left, IExpression right)
		{
			if (left.Equals(BoolConstExpression.True) || right.Equals(BoolConstExpression.True
				))
			{
				return BoolConstExpression.True;
			}
			if (left.Equals(BoolConstExpression.False))
			{
				return right;
			}
			if (right.Equals(BoolConstExpression.False))
			{
				return left;
			}
			if (left.Equals(right))
			{
				return left;
			}
			if (Negatives(left, right))
			{
				return BoolConstExpression.True;
			}
			return new OrExpression(left, right);
		}

		/// <summary>Optimizations: static bool roots</summary>
		public virtual BoolConstExpression Constant(bool value)
		{
			return BoolConstExpression.Expr(value);
		}

		public virtual IExpression IfThenElse(IExpression cond, IExpression truePath, IExpression
			 falsePath)
		{
			IExpression expr = CheckBoolean(cond, truePath, falsePath);
			if (expr != null)
			{
				return expr;
			}
			return Or(And(cond, truePath), And(Not(cond), falsePath));
		}

		private IExpression CheckBoolean(IExpression cmp, IExpression trueExpr, IExpression
			 falseExpr)
		{
			if (cmp is BoolConstExpression)
			{
				return null;
			}
			if (trueExpr is BoolConstExpression)
			{
				bool leftNegative = trueExpr.Equals(BoolConstExpression.False);
				if (!leftNegative)
				{
					return Or(cmp, falseExpr);
				}
				else
				{
					return And(Not(cmp), falseExpr);
				}
			}
			if (falseExpr is BoolConstExpression)
			{
				bool rightNegative = falseExpr.Equals(BoolConstExpression.False);
				if (!rightNegative)
				{
					return And(cmp, trueExpr);
				}
				else
				{
					return Or(Not(cmp), falseExpr);
				}
			}
			if (cmp is NotExpression)
			{
				cmp = ((NotExpression)cmp).Expr();
				IExpression swap = trueExpr;
				trueExpr = falseExpr;
				falseExpr = swap;
			}
			if (trueExpr is OrExpression)
			{
				OrExpression orExpr = (OrExpression)trueExpr;
				IExpression orLeft = orExpr.Left();
				IExpression orRight = orExpr.Right();
				if (falseExpr.Equals(orRight))
				{
					IExpression swap = orRight;
					orRight = orLeft;
					orLeft = swap;
				}
				if (falseExpr.Equals(orLeft))
				{
					return Or(orLeft, And(cmp, orRight));
				}
			}
			if (falseExpr is AndExpression)
			{
				AndExpression andExpr = (AndExpression)falseExpr;
				IExpression andLeft = andExpr.Left();
				IExpression andRight = andExpr.Right();
				if (trueExpr.Equals(andRight))
				{
					IExpression swap = andRight;
					andRight = andLeft;
					andLeft = swap;
				}
				if (trueExpr.Equals(andLeft))
				{
					return And(andLeft, Or(cmp, andRight));
				}
			}
			return null;
		}

		private bool Negatives(IExpression left, IExpression right)
		{
			return NegativeOf(left, right) || NegativeOf(right, left);
		}

		private bool NegativeOf(IExpression right, IExpression left)
		{
			return (right is NotExpression) && ((NotExpression)right).Expr().Equals(left);
		}
	}
}
