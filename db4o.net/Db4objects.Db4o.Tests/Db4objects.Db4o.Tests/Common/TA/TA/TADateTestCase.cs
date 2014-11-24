/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA.TA;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	public class TADateTestCase : TAItemTestCaseBase
	{
		public static DateTime first = new DateTime(1195401600000L);

		public static void Main(string[] args)
		{
			new TADateTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			TADateItem item = (TADateItem)obj;
			Assert.AreEqual(first, item.GetUntyped());
			Assert.AreEqual(first, item.GetTyped());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertRetrievedItem(object obj)
		{
			TADateItem item = (TADateItem)obj;
			Assert.IsNull(item._untyped);
			Assert.AreEqual(EmptyValue(), item._typed);
		}

		private object EmptyValue()
		{
			return Db().Reflector().ForClass(typeof(DateTime)).NullValue();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			TADateItem item = new TADateItem();
			item._typed = first;
			item._untyped = first;
			return item;
		}
	}
}
