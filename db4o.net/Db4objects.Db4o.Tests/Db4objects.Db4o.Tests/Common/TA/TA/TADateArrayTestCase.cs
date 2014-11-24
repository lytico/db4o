/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA.TA;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	public class TADateArrayTestCase : TAItemTestCaseBase
	{
		public static readonly DateTime[] data = new DateTime[] { new DateTime(0), new DateTime
			(1), new DateTime(1191972104500L) };

		public static void Main(string[] args)
		{
			new TADateArrayTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			TADateArrayItem item = (TADateArrayItem)obj;
			for (int i = 0; i < data.Length; i++)
			{
				Assert.AreEqual(data[i], item.GetTyped()[i]);
				Assert.AreEqual(data[i], (DateTime)item.GetUntyped()[i]);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			TADateArrayItem item = new TADateArrayItem();
			item._typed = new DateTime[data.Length];
			item._untyped = new object[data.Length];
			System.Array.Copy(data, 0, item._typed, 0, data.Length);
			System.Array.Copy(data, 0, item._untyped, 0, data.Length);
			return item;
		}
	}
}
