/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	/// <exclude></exclude>
	public class IntArrayListTestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(IntArrayListTestCase)).Run();
		}

		public virtual void TestIteratorGoesForwards()
		{
			IntArrayList list = new IntArrayList();
			AssertIterator(new int[] {  }, list.IntIterator());
			list.Add(1);
			AssertIterator(new int[] { 1 }, list.IntIterator());
			list.Add(2);
			AssertIterator(new int[] { 1, 2 }, list.IntIterator());
		}

		private void AssertIterator(int[] expected, IIntIterator4 iterator)
		{
			for (int i = 0; i < expected.Length; ++i)
			{
				Assert.IsTrue(iterator.MoveNext());
				Assert.AreEqual(expected[i], iterator.CurrentInt());
				Assert.AreEqual(expected[i], iterator.Current);
			}
			Assert.IsFalse(iterator.MoveNext());
		}

		//test mthod add(int,int)
		public virtual void TestAddAtIndex()
		{
			IntArrayList list = new IntArrayList();
			for (int i = 0; i < 10; i++)
			{
				list.Add(i);
			}
			list.Add(3, 100);
			Assert.AreEqual(100, list.Get(3));
			for (int i = 4; i < 11; i++)
			{
				Assert.AreEqual(i - 1, list.Get(i));
			}
		}
	}
}
