/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System.Linq.Expressions;
using Db4objects.Db4o.Linq.Internals;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Linq.Expressions
{
	internal abstract class OrderByClauseVisitorBase : ExpressionQueryBuilder
	{
		protected abstract void ApplyDirection(IQuery query);

		protected override void VisitMethodCall(MethodCallExpression methodCall)
		{
			Visit(methodCall.Object);
			AnalyseMethod(Recorder, methodCall.Method);
		}

		protected override void VisitMemberAccess(MemberExpression m)
		{
			ProcessMemberAccess(m);
		}

		public override IQueryBuilderRecord Process(LambdaExpression expression)
		{
			if (!StartsWithParameterReference(expression.Body))
				CannotOptimize(expression.Body);

			return ApplyDirection(base.Process(expression));
		}

		private IQueryBuilderRecord ApplyDirection(IQueryBuilderRecord record)
		{
			QueryBuilderRecorder recorder = new QueryBuilderRecorder(record);
			recorder.Add(ctx => ApplyDirection(ctx.CurrentQuery));
			return recorder.Record;
		}
	}
}
