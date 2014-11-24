/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Regression
{
	public class DRS42Test : DrsTestCase
	{
		internal NewPilot andrew = new NewPilot("Andrew", 100, new int[] { 100, 200, 300 }
			);

		public virtual void Test()
		{
			StoreToProviderA();
			ReplicateAllToProviderB();
		}

		internal virtual void StoreToProviderA()
		{
			ITestableReplicationProviderInside provider = A().Provider();
			provider.StoreNew(andrew);
			provider.Commit();
			EnsureContent(andrew, provider);
		}

		internal virtual void ReplicateAllToProviderB()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureContent(andrew, B().Provider());
		}

		private void EnsureContent(NewPilot newPilot, ITestableReplicationProviderInside 
			provider)
		{
			IObjectSet objectSet = provider.GetStoredObjects(typeof(NewPilot));
			Assert.AreEqual(1, objectSet.Count);
			IEnumerator iterator = objectSet.GetEnumerator();
			Assert.IsTrue(iterator.MoveNext());
			NewPilot p = (NewPilot)iterator.Current;
			Assert.AreEqual(newPilot.GetName(), p.GetName());
			Assert.AreEqual(newPilot.GetPoints(), p.GetPoints());
			for (int j = 0; j < newPilot.GetArr().Length; j++)
			{
				Assert.AreEqual(newPilot.GetArr()[j], p.GetArr()[j]);
			}
		}
	}
}
