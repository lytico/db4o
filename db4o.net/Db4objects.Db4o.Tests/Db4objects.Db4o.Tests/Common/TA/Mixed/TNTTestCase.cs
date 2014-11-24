/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	/// <exclude></exclude>
	public class TNTTestCase : ItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new TNTTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			return new TNTItem(42);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertRetrievedItem(object obj)
		{
			TNTItem item = (TNTItem)obj;
			Assert.IsNull(item.ntItem);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			TNTItem item = (TNTItem)obj;
			NTItem ntItem = item.Value();
			Assert.IsNotNull(ntItem);
			Assert.IsNotNull(ntItem.tItem);
			Assert.AreEqual(0, ntItem.tItem.value);
			Assert.AreEqual(42, ntItem.tItem.Value());
		}
	}
}
