/* Copyright (C) 2010 Versant Inc.   http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Activation;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Internal.Activation
{
	public class ActivatableBaseTestCase : AbstractDb4oTestCase
	{
		protected override void Store()
		{
			Store(new Item());
		}
		
		public void TestNoClassIndex()
		{
			IStoredClass storedClass = Db().StoredClass(typeof (ActivatableBase));
			Assert.IsNotNull(storedClass);
			Assert.IsFalse(storedClass.HasClassIndex());
		}
	}

	public class Item : ActivatableBase
	{
	}
}
