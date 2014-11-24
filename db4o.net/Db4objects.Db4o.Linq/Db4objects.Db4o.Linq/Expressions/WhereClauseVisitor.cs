/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */
using System;
using System.Linq.Expressions;
using Db4objects.Db4o.Linq.Caching;
using Db4objects.Db4o.Linq.Internals;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq.Expressions
{
	internal class WhereClauseVisitor : ExpressionQueryBuilder
	{
		private static readonly ICache4<Expression, IQueryBuilderRecord> _cache = ExpressionCacheFactory.NewInstance(42);

		protected override ICache4<Expression, IQueryBuilderRecord> GetCachingStrategy()
		{
			return _cache;
		}

		protected override void VisitMethodCall(MethodCallExpression m)
		{
			Visit(m.Object);
			VisitExpressionList(m.Arguments);

			if (OptimizeableMethodConstrains.IsStringMethod(m.Method))
			{
				ProcessStringMethod(m);
				return;
			}

			if (OptimizeableMethodConstrains.IsIListOrICollectionOfTMethod(m.Method))
			{
				ProcessCollectionMethod(m);
				return;
			}

			AnalyseMethod(Recorder, m.Method);
		}

		private void ProcessStringMethod(MethodCallExpression call)
		{
			switch (call.Method.Name)
			{
				case "EndsWith":
					RecordConstraintApplication(c => c.EndsWith(true));
					return;

				case "StartsWith":
					RecordConstraintApplication(c => c.StartsWith(true));
					return;

				case "Contains":
					RecordConstraintApplication(c => c.Contains());
					return;

				case "Equals":
					return;
			}

			CannotOptimize(call);
		}

		private void RecordConstraintApplication(Func<IConstraint, IConstraint> application)
		{
			Recorder.Add(ctx => ctx.ApplyConstraint(application));
		}

		private void ProcessCollectionMethod(MethodCallExpression call)
		{
			switch (call.Method.Name)
			{
				case "Contains":
					if (IsCallOnCollectionOfStrings(call))
					{
						RecordConstraintApplication(c => c.Contains());
					}
					return;
			}

			CannotOptimize(call);
		}

		private static bool IsCallOnCollectionOfStrings(MethodCallExpression call)
		{
			return call.Method.DeclaringType.IsGenericType && call.Method.DeclaringType.GetGenericArguments()[0] == typeof(string);
		}

		private static bool IsComparisonExpression(Expression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
					return true;
				default:
					return false;
			}
		}

		private static bool IsConditionalExpression(Expression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.AndAlso:
				case ExpressionType.OrElse:
					return true;
				default:
					return false;
			}
		}

		protected override void VisitBinary(BinaryExpression b)
		{
			if (IsConditionalExpression(b))
			{
				ProcessConditionalExpression(b);
				return;
			}

			if (IsComparisonExpression(b))
			{
				ProcessPredicateExpression(b);
				return;
			}

			CannotOptimize(b);
		}

		protected override void VisitUnary(UnaryExpression u)
		{
			var operand = u.Operand;
			if (u.NodeType == ExpressionType.Not)
			{
				Visit(operand);
				RecordConstraintApplication(c => c.Not());
				return;
			}

			if (u.NodeType == ExpressionType.Convert)
			{
				Visit(operand);
				return;
			}

			CannotOptimize(u);
		}

		private void ProcessConditionalExpression(BinaryExpression b)
		{
			VisitPreservingQuery(b.Left);
			VisitPreservingQuery(b.Right);

			switch (b.NodeType)
			{
				case ExpressionType.AndAlso:
					Recorder.Add(ctx => ctx.ApplyConstraint(c => c.And(ctx.PopConstraint())));
					break;
				case ExpressionType.OrElse:
					Recorder.Add(ctx => ctx.ApplyConstraint(c => c.Or(ctx.PopConstraint())));
					break;
			}
		}

		private void VisitPreservingQuery(Expression expression)
		{
			PreservingQuery(() => Visit(expression));
		}

		private void PreservingQuery(Action action)
		{
			Recorder.Add(ctx => ctx.SaveQuery());
			action();
			Recorder.Add(ctx => ctx.RestoreQuery());
		}

		private void ProcessPredicateExpression(BinaryExpression b)
		{
			if (ParameterReferenceOnLeftSide(b))
			{
				Visit(b.Left);
				Visit(b.Right);
			}
			else
			{
				Visit(b.Right);
				Visit(b.Left);
			}

			ProcessPredicateExpressionOperator(b);
		}

		protected override void VisitMemberAccess(MemberExpression m)
		{
			if (!StartsWithParameterReference(m)) CannotOptimize(m);

			ProcessMemberAccess(m);
		}

		protected override void VisitConstant(ConstantExpression c)
		{
			var value = c.Value;
			Recorder.Add(ctx => ctx.PushConstraint(ctx.CurrentQuery.Constrain(ctx.ResolveValue(value))));
		}

		static bool ParameterReferenceOnLeftSide(BinaryExpression b)
		{
			if (StartsWithParameterReference(b.Left)) return true;
			if (StartsWithParameterReference(b.Right)) return false;

			CannotOptimize(b);
			return false;
		}

		private void ProcessPredicateExpressionOperator(BinaryExpression b)
		{
			switch (b.NodeType)
			{
				case ExpressionType.Equal:
					break;
				case ExpressionType.NotEqual:
					RecordConstraintApplication(c => c.Not());
					break;
				case ExpressionType.LessThan:
					RecordConstraintApplication(c => c.Smaller());
					break;
				case ExpressionType.LessThanOrEqual:
					RecordConstraintApplication(c => c.Smaller().Equal());
					break;
				case ExpressionType.GreaterThan:
					RecordConstraintApplication(c => c.Greater());
					break;
				case ExpressionType.GreaterThanOrEqual:
					RecordConstraintApplication(c => c.Greater().Equal());
					break;
				default:
					CannotOptimize(b);
					break;
			}
		}
	}
}
