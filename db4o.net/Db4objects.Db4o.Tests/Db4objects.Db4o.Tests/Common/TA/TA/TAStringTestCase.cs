/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA.TA;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	/// <exclude></exclude>
	public class TAStringTestCase : TAItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new TAStringTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			TAStringItem item = new TAStringItem();
			item.value = "42";
			item.obj = "hello";
			return item;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			TAStringItem item = (TAStringItem)obj;
			Assert.AreEqual("42", item.Value());
			Assert.AreEqual("hello", item.Object());
		}

		protected override void AssertRetrievedItem(object obj)
		{
			TAStringItem item = (TAStringItem)obj;
			Assert.IsNull(item.value);
			Assert.IsNull(item.obj);
		}
	}
}
