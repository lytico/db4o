/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Util.Test
{
	public class PermutingTestConfigTestCase : ITestCase
	{
		public virtual void TestPermutation()
		{
			object[][] data = new object[][] { new object[] { "A", "B" }, new object[] { "X", 
				"Y", "Z" } };
			PermutingTestConfig config = new PermutingTestConfig(data);
			object[][] expected = new object[][] { new object[] { "A", "X" }, new object[] { 
				"A", "Y" }, new object[] { "A", "Z" }, new object[] { "B", "X" }, new object[] { 
				"B", "Y" }, new object[] { "B", "Z" } };
			for (int groupIdx = 0; groupIdx < expected.Length; groupIdx++)
			{
				Assert.IsTrue(config.MoveNext());
				object[] current = new object[] { config.Current(0), config.Current(1) };
				ArrayAssert.AreEqual(expected[groupIdx], current);
			}
			Assert.IsFalse(config.MoveNext());
		}
	}
}
