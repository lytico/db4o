/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class SameHashCodeTestCase : EqualsHashCodeOverriddenTestCaseBase
	{
		public class Holder
		{
			internal EqualsHashCodeOverriddenTestCaseBase.Item _itemA;

			internal EqualsHashCodeOverriddenTestCaseBase.Item _itemB;

			public Holder(EqualsHashCodeOverriddenTestCaseBase.Item itemA, EqualsHashCodeOverriddenTestCaseBase.Item
				 itemB)
			{
				_itemA = itemA;
				_itemB = itemB;
			}
		}

		public virtual void TestReplicatesSimpleHolder()
		{
			AssertReplicates(new SameHashCodeTestCase.Holder(new EqualsHashCodeOverriddenTestCaseBase.Item
				("item"), new EqualsHashCodeOverriddenTestCaseBase.Item("item")));
		}
	}
}
