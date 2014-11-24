/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Instrumentation.Cecil;
using Db4objects.Db4o.NativeQueries;
using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Instrumentation;
using Db4oTool.Core;
using Mono.Cecil;

namespace Db4oTool.NQ
{
	public abstract class AbstractOptimizer : AbstractAssemblyInstrumentation
	{
		public void OptimizePredicate(TypeDefinition type, MethodDefinition match, IExpression e)
		{
			TraceInfo("Optimizing '{0}' ({1})", type, e);

			new SODAMethodBuilder(new CecilTypeEditor(type)).InjectOptimization(e);
		}

		public IExpression GetExpression(MethodDefinition match)
		{
			try
			{
				return QueryExpressionBuilder.FromMethodDefinition(match);
			}
			catch (Exception x)
			{
				TraceWarning("WARNING: Predicate '{0}' could not be optimized. {1}", match, x.Message);
				TraceVerbose("{0}", x);
			}
			return null;
		}

		protected override void BeforeAssemblyProcessing()
		{
			_processedCount = 0;
		}

		protected override void AfterAssemblyProcessing()
		{
			TraceInfo("{0} {1} processed.", _processedCount, TargetName(_processedCount));
		}

		protected abstract string TargetName(int count);

		protected int _processedCount;
	}
}