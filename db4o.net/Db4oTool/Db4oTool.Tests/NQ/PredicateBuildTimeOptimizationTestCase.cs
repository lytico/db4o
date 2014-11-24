/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using Db4oTool.Tests.Core;
using Db4objects.Db4o.Internal.Query;
using Db4oUnit;

namespace Db4oTool.Tests.NQ
{
	public class PredicateBuildTimeOptimizationTestCase : SingleResourceTestCase
	{
		// FIXME: make it work in release mode too
		protected override bool AcceptsReleaseMode
		{
			get { return false; }
		}

		protected override string ResourceName
		{
			get { return "PredicateSubject"; }
		}

		protected override string CommandLine
		{
			get { return "-nq"; }
		}

		protected override void OnQueryExecution(object sender, QueryExecutionEventArgs args)
		{
			Assert.AreEqual(QueryExecutionKind.PreOptimized, args.ExecutionKind);
		}
	}
}
