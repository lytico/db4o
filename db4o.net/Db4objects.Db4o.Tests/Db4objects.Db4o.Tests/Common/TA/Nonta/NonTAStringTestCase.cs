/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA.Nonta;

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	/// <exclude></exclude>
	public class NonTAStringTestCase : NonTAItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new NonTAStringTestCase().RunAll();
		}

		protected override void AssertItemValue(object obj)
		{
			StringItem item = (StringItem)obj;
			Assert.AreEqual("42", item.Value());
			Assert.AreEqual("hello", item.Object());
		}

		protected override object CreateItem()
		{
			StringItem item = new StringItem();
			item.value = "42";
			item.obj = "hello";
			return item;
		}
	}
}
