/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Drs.Foundation;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class UuidConversionTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			SPCChild child = StoreInA();
			Replicate();
			IReplicationReference @ref = A().Provider().ProduceReference(child);
			B().Provider().ClearAllReferences();
			IDrsUUID expectedUuid = @ref.Uuid();
			IReplicationReference referenceByUUID = B().Provider().ProduceReferenceByUUID(expectedUuid
				, null);
			Assert.IsNotNull(referenceByUUID);
			IDrsUUID actualUuid = referenceByUUID.Uuid();
			Assert.AreEqual(expectedUuid.GetLongPart(), actualUuid.GetLongPart());
		}

		private SPCChild StoreInA()
		{
			string name = "c1";
			SPCChild child = CreateChildObject(name);
			A().Provider().StoreNew(child);
			A().Provider().Commit();
			return child;
		}

		private void Replicate()
		{
			ReplicateAll(A().Provider(), B().Provider());
		}

		protected virtual SPCChild CreateChildObject(string name)
		{
			return new SPCChild(name);
		}
	}
}
