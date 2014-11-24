/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Staging;

namespace Db4objects.Drs.Tests.Staging
{
	public class MapElementCustomHashCodeTestCase : EqualsHashCodeOverriddenTestCaseBase
	{
		public class Holder
		{
			internal IDictionary _map = new Hashtable();

			public Holder(EqualsHashCodeOverriddenTestCaseBase.Item itemA, EqualsHashCodeOverriddenTestCaseBase.Item
				 itemB)
			{
				// DRS-118
				// NOTE: This test does not necessarily reproduce the symptom.
				_map[itemA] = itemA;
				_map[itemB] = itemB;
			}
		}

		public virtual void TestReplicatesMap()
		{
			AssertReplicates(new MapElementCustomHashCodeTestCase.Holder(new EqualsHashCodeOverriddenTestCaseBase.Item
				("item"), new EqualsHashCodeOverriddenTestCaseBase.Item("item")));
		}
	}
}
