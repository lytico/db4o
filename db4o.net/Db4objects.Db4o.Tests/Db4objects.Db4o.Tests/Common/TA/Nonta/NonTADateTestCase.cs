/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA.Nonta;

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	public class NonTADateTestCase : NonTAItemTestCaseBase
	{
		public static DateTime first = new DateTime(1195401600000L);

		public static void Main(string[] args)
		{
			new NonTADateTestCase().RunAll();
		}

		protected override void AssertItemValue(object obj)
		{
			DateItem item = (DateItem)obj;
			Assert.AreEqual(first, item._untyped);
			Assert.AreEqual(first, item._typed);
		}

		protected override object CreateItem()
		{
			DateItem item = new DateItem();
			item._typed = first;
			item._untyped = first;
			return item;
		}
	}
}
