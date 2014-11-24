/* Copyright (C) 2007 - 2010  Versant Inc.  http://www.db4o.com */

#if CF || SILVERLIGHT

using System;
using System.Reflection;
using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.Linq.Caching;
using Db4objects.Db4o.Linq.Internals;
using Cecil.FlowAnalysis;
using Cecil.FlowAnalysis.ActionFlow;
using Cecil.FlowAnalysis.CodeStructure;

using Mono.Cecil;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal class CecilMethodAnalyser : IMethodAnalyser
	{
		private static ICache4<MethodDefinition, ActionFlowGraph> _graphCache =
			CacheFactory<MethodDefinition, ActionFlowGraph>.For(CacheFactory.New2QXCache(5));

		private readonly Expression _queryExpression;

		private CecilMethodAnalyser(ActionFlowGraph graph)
		{
			if (graph == null) throw new ArgumentNullException("graph");

			_queryExpression = QueryExpressionFinder.FindIn(graph);
		}

		public void Run(QueryBuilderRecorder recorder)
		{
			if (_queryExpression == null) throw new QueryOptimizationException("No query expression");

			_queryExpression.Accept(new CodeQueryBuilder(recorder));
		}

		public static IMethodAnalyser FromMethod(MethodInfo info)
		{
			return GetAnalyserFor(ResolveMethod(info));
		}

		private static MethodDefinition ResolveMethod(MethodInfo info)
		{
			if (info == null) throw new ArgumentNullException("info");

			var method = MetadataResolver.Instance.ResolveMethod(info);

			if (method == null) throw new QueryOptimizationException(
				string.Format("Cannot resolve method {0}", info));

			return method;
		}

		private static IMethodAnalyser GetAnalyserFor(MethodDefinition method)
		{
			var graph = _graphCache.Produce(method, CreateActionFlowGraph);
			return new CecilMethodAnalyser(graph);
		}

		private static ActionFlowGraph CreateActionFlowGraph(MethodDefinition method)
		{
			return FlowGraphFactory.CreateActionFlowGraph(FlowGraphFactory.CreateControlFlowGraph(method));
		}
	}
}

#endif
