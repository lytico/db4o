/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using System.Linq.Expressions;
using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq.Expressions
{
	class ExpressionTreeNormalizer : ExpressionTransformer
	{
		protected override Expression VisitLambda(LambdaExpression lambda)
		{
			if (IsBooleanMemberAccess(lambda.Body))
			{
				return Expression.Lambda(ExpandExpression(lambda.Body, true));
			}

			return base.VisitLambda(lambda);
		}

		protected override Expression VisitUnary(UnaryExpression u)
		{
			if (u.NodeType != ExpressionType.Not)
			{
				return base.VisitUnary(u);
			}

			if (IsBooleanMemberAccess(u.Operand) || IsNonOptimizeableBooleanMethodCall(u.Operand))
			{
				return ExpandExpression(u.Operand, false);
			}

			return base.VisitUnary(u);
		}

		protected override Expression VisitBinary(BinaryExpression b)
		{
			var expression = NormalizeBooleanMemberAccess(b);
			return NormalizeVisualBasicOperator(expression) ?? base.VisitBinary(expression);
		}

		protected override Expression VisitMethodCall(MethodCallExpression method)
		{
			Visit(method.Object);
			VisitExpressionList(method.Arguments);
			if (IsNonOptimizeableBooleanMethodCall(method))
			{
				return ExpandExpression(method, true);
			}

			return base.VisitMethodCall(method);
		}

		private static bool IsNonOptimizeableBooleanMethodCall(Expression expression)
		{
			return expression.NodeType == ExpressionType.Call
					&& expression.Type == typeof(bool)
					&& !IsOptimizeableMethodCall((MethodCallExpression) expression);
		}

		private static bool IsOptimizeableMethodCall(MethodCallExpression expression)
		{
			return OptimizeableMethodConstrains.CanBeOptimized(expression.Method);
		}

		private static BinaryExpression ExpandExpression(Expression expression, bool value)
		{
			return Expression.Equal(expression, Expression.Constant(value));
		}

		private static bool IsBooleanMemberAccess(Expression expression)
		{
			return expression.NodeType == ExpressionType.MemberAccess && expression.Type == typeof(bool);
		}

		private static BinaryExpression NormalizeBooleanMemberAccess(BinaryExpression expression)
		{
			if (!IsLogicalOperator(expression))
				return expression;

			if (IsBooleanMemberAccess(expression.Right))
			{
				expression = Expression.MakeBinary(expression.NodeType, expression.Left, ExpandExpression(expression.Right, true));
			}

			if (IsBooleanMemberAccess(expression.Left))
			{
				expression = Expression.MakeBinary(expression.NodeType, ExpandExpression(expression.Left, true), expression.Right);
			}
			return expression;
		}

		private static bool IsLogicalOperator(Expression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.OrElse:
				case ExpressionType.AndAlso:
					return true;

				default:
					return false;
			}
		}

		private static Expression NormalizeVisualBasicOperator(BinaryExpression b)
		{
			var call = b.Left as MethodCallExpression;
			if (call == null) return null;
			if (call.Object != null) return null;
			if (call.Method.DeclaringType.FullName != "Microsoft.VisualBasic.CompilerServices.Operators") return null;

			switch (call.Method.Name)
			{
				case "CompareString":
					{
						switch (b.NodeType)
						{
							case ExpressionType.Equal:
								return ToStringEquals(call);
							case ExpressionType.NotEqual:
								return Expression.Not(ToStringEquals(call));
						}

						return null;
					}
			}
			return null;
		}

		private static MethodCallExpression ToStringEquals(MethodCallExpression call)
		{
			var stringEquals = typeof(string).GetMethod("Equals", new[] {typeof(string)});
			return Expression.Call(call.Arguments[0], stringEquals, call.Arguments[1]);
		}

		public Expression Normalize(Expression expression)
		{
			return Visit(expression);
		}
	}
}
