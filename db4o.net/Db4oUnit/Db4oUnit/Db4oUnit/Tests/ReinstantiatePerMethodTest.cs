/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;

namespace Db4oUnit.Tests
{
	public class ReinstantiatePerMethodTest : ITestCase
	{
		private int a = 0;

		public virtual void Test1()
		{
			Assert.AreEqual(0, a);
			a = 1;
		}

		public virtual void Test2()
		{
			Assert.AreEqual(0, a);
			a = 2;
		}
	}
}
