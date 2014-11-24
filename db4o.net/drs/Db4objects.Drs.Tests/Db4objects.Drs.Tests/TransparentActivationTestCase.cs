/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;
using Sharpen;

namespace Db4objects.Drs.Tests
{
	public class TransparentActivationTestCase : DrsTestCase
	{
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			ActivatableItem item = new ActivatableItem("foo");
			A().Provider().StoreNew(item);
			A().Provider().Commit();
			if (A().Provider() is IDb4oReplicationProvider)
			{
				// TODO: We can't reopen Hibernate providers here if
				// they run on an in-memory database.
				// db4o should be reopened, otherwise Transparent Activation
				// is not tested.
				Reopen();
			}
			ReplicateAll(A().Provider(), B().Provider());
			Runtime.Gc();
			// Improve chances TA is really required
			IObjectSet items = B().Provider().GetStoredObjects(typeof(ActivatableItem));
			Assert.IsTrue(items.HasNext());
			ActivatableItem replicatedItem = (ActivatableItem)items.Next();
			Assert.AreEqual(item.Name(), replicatedItem.Name());
		}
	}
}
