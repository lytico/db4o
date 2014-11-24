/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Drs.Tests.Foundation;

namespace Db4objects.Drs.Tests.Foundation
{
	public class Set4Testcase : ITestCase
	{
		public virtual void TestSingleElementIteration()
		{
			Set4 set = NewSet("first");
			Assert.AreEqual("first", Iterators.Next(set.GetEnumerator()));
		}

		public virtual void TestContainsAll()
		{
			Set4 set = NewSet("42");
			set.Add("foo");
			Assert.IsTrue(set.ContainsAll(NewSet("42")));
			Assert.IsTrue(set.ContainsAll(NewSet("foo")));
			Assert.IsTrue(set.ContainsAll(set));
			Set4 other = new Set4(set);
			other.Add("bar");
			Assert.IsFalse(set.ContainsAll(other));
		}

		private Set4 NewSet(string firstElement)
		{
			Set4 set = new Set4();
			set.Add(firstElement);
			return set;
		}
	}
}
