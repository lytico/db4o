/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA.TA;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	/// <exclude></exclude>
	public class TAIntTestCase : TAItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new TAIntTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			TAIntItem item = new TAIntItem();
			item.value = 42;
			item.i = 1;
			item.obj = 2;
			return item;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			TAIntItem item = (TAIntItem)obj;
			Assert.AreEqual(42, item.Value());
			Assert.AreEqual(1, item.IntegerValue());
			Assert.AreEqual(2, item.Object());
		}
	}
}
