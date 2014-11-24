/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;
using Sharpen;

namespace Db4objects.Drs.Tests
{
	public class DateReplicationTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			ItemDates item1 = new ItemDates(new DateTime(0), new DateTime());
			ItemDates item2 = new ItemDates(new DateTime(10000), new DateTime(Runtime.CurrentTimeMillis
				() - 10000));
			A().Provider().StoreNew(item1);
			A().Provider().StoreNew(item2);
			A().Provider().Commit();
			ReplicateAll(A().Provider(), B().Provider());
			IObjectSet found = B().Provider().GetStoredObjects(typeof(ItemDates));
			Iterator4Assert.SameContent(new object[] { item2, item1 }, ReplicationTestPlatform
				.Adapt(found.GetEnumerator()));
		}
	}
}
