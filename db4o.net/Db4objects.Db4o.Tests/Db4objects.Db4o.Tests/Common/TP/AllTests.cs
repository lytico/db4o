/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.TP;

namespace Db4objects.Db4o.Tests.Common.TP
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(CommitTimeStampsWithTPTestCase), typeof(DeactivateDeletedObjectOnRollbackStrategyTestCase
				), typeof(QueryConsistencyTestCase), typeof(RollbackStrategyTestCase), typeof(TransparentPersistenceTestCase
				) };
		}
	}
}
