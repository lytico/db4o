/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class UnknownReferenceDeactivationTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public string value;

			public Item(string value)
			{
				this.value = value;
			}
		}

		public virtual void Test()
		{
			UnknownReferenceDeactivationTestCase.Item item = new UnknownReferenceDeactivationTestCase.Item
				("my string");
			Db().Deactivate(item, int.MaxValue);
			Assert.AreEqual("my string", item.value);
		}
	}
}
