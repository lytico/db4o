/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class TPExplicitStoreFieldIndexConsistencyTestCase : TPFieldIndexConsistencyTestCaseBase
	{
		public virtual void TestExplicitStore()
		{
			int id = 42;
			TPFieldIndexConsistencyTestCaseBase.Item item = new TPFieldIndexConsistencyTestCaseBase.Item
				(id);
			Store(item);
			Store(item);
			AssertItemQuery(id);
			Commit();
			AssertFieldIndex(id);
		}
	}
}
