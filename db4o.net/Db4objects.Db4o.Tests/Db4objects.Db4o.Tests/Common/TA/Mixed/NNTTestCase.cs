/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	/// <exclude></exclude>
	public class NNTTestCase : ItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new NNTTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			return new NNTItem(42);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertRetrievedItem(object obj)
		{
			NNTItem item = (NNTItem)obj;
			Assert.IsNotNull(item.ntItem);
			Assert.IsNotNull(item.ntItem.tItem);
			Assert.AreEqual(0, item.ntItem.tItem.value);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			NNTItem item = (NNTItem)obj;
			Assert.AreEqual(42, item.ntItem.tItem.Value());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeactivateDepth()
		{
			NNTItem item = (NNTItem)RetrieveOnlyInstance();
			NTItem ntItem = item.ntItem;
			TItem tItem = ntItem.tItem;
			tItem.Value();
			// item.ntItem.tItem.value
			Assert.IsNotNull(ntItem.tItem);
			Db().Deactivate(item, 2);
			// FIXME: failure 
			//		Assert.isNull(ntItem.tItem);
			Db().Activate(item, 42);
			Db().Deactivate(item, 3);
			Assert.IsNull(ntItem.tItem);
		}
	}
}
