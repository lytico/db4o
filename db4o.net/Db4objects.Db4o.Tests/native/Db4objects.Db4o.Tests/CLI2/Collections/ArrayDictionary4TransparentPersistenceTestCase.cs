/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using Db4objects.Db4o.Collections;
using Db4objects.Db4o.Tests.Common.TA;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
	public class ArrayDictionary4TransparentPersistenceTestCase : ITestLifeCycle
	{
		private ArrayDictionary4<string, int> dict;
		private MockActivator activator;

		public void TestRemove()
		{
			dict.Remove("foo"); 
			Assert.AreEqual(0, activator.WriteCount(), "removing non-existent element");

			dict.Remove("ltuae");
			Assert.AreEqual(1, activator.WriteCount());
		}

		public void TestRemovePair()
		{
			ICollection<KeyValuePair<string, int>> pairs = (ICollection<KeyValuePair<string, int>>) dict;
			Assert.IsFalse(pairs.Remove(new KeyValuePair<string, int>("ltuae", 41)));

			Assert.AreEqual(0, activator.WriteCount(), "removing non-existent element");

			Assert.IsTrue(pairs.Remove(new KeyValuePair<string, int>("ltuae", 42)));
			Assert.AreEqual(1, activator.WriteCount());
		}

		public void TestIndexer()
		{
			dict["ltuae"] = 44;
			Assert.AreEqual(1, activator.WriteCount(), "changing existing value");

			dict["2+2"] = 4;
			Assert.AreEqual(2, activator.WriteCount(), "adding new value");
		}

		public void SetUp()
		{
			dict = CreateDictionary();
			activator = MockActivator.ActivatorFor(dict);
		}

		public void TearDown()
		{
		}

		private static ArrayDictionary4<string, int> CreateDictionary()
		{
			ArrayDictionary4<string, int> dict = new ArrayDictionary4<string, int>();
			dict["ltuae"] = 42;
			return dict;
		}
	}
}
