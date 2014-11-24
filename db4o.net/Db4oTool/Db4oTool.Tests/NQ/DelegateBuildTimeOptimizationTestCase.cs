/* Copyright (C) 2004 - 2006  Versant Inc.   http://www.db4o.com */
using Db4oTool.Tests.Core;
using Db4objects.Db4o.Internal.Query;
using Db4oUnit;

namespace Db4oTool.Tests.NQ
{
	using System;

	public class DelegateBuildTimeOptimizationTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "DelegateSubject"; }
		}

		protected override string CommandLine
		{
			get { return "-nq"; }
		}

		override protected void OnQueryExecution(object sender, QueryExecutionEventArgs args)
		{
			Assert.AreEqual(QueryExecutionKind.PreOptimized, args.ExecutionKind);
		}
	}
}