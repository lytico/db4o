/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class CustomArrayListTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			NamedList original = new NamedList("foo");
			original.Add("bar");
			A().Provider().StoreNew(original);
			A().Provider().Commit();
			ReplicateAll(A().Provider(), B().Provider());
			IEnumerator iterator = B().Provider().GetStoredObjects(typeof(NamedList)).GetEnumerator
				();
			Assert.IsTrue(iterator.MoveNext());
			NamedList replicated = (NamedList)iterator.Current;
			Assert.AreEqual(original.Name(), replicated.Name());
			CollectionAssert.AreEqual(original, replicated);
		}
	}
}
