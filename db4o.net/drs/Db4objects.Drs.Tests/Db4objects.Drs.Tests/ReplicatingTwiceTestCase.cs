/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class ReplicatingTwiceTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			Pilot pilot = new Pilot("one", 1);
			A().Provider().StoreNew(pilot);
			A().Provider().Commit();
			ReplicateAll(A().Provider(), B().Provider(), null);
			pilot.SetName("modified");
			A().Provider().Update(pilot);
			A().Provider().Commit();
			ReplicateAll(A().Provider(), B().Provider(), null);
			Pilot pilotFromB = (Pilot)GetOneInstance(B(), typeof(Pilot));
			Assert.AreEqual("modified", pilotFromB.Name());
		}
	}
}
