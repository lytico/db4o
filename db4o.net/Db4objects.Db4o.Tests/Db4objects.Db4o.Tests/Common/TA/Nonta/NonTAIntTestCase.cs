/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA.Nonta;

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	/// <exclude></exclude>
	public class NonTAIntTestCase : NonTAItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new NonTAIntTestCase().RunAll();
		}

		protected override void AssertItemValue(object obj)
		{
			IntItem item = (IntItem)obj;
			Assert.AreEqual(42, item.Value());
			Assert.AreEqual(1, item.IntegerValue());
			Assert.AreEqual(2, item.Object());
		}

		protected override object CreateItem()
		{
			IntItem item = new IntItem();
			item.value = 42;
			item.i = 1;
			item.obj = 2;
			return item;
		}
	}
}
