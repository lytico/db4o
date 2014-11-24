/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	/// <exclude></exclude>
	public class NTNTestCase : ItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new NTNTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			return new NTNItem(42);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertRetrievedItem(object obj)
		{
			NTNItem item = (NTNItem)obj;
			Assert.IsNotNull(item.tnItem);
			Assert.IsNull(item.tnItem.list);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			NTNItem item = (NTNItem)obj;
			Assert.AreEqual(LinkedList.NewList(42), item.tnItem.Value());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeactivateDepth()
		{
			NTNItem item = (NTNItem)RetrieveOnlyInstance();
			TNItem tnItem = item.tnItem;
			tnItem.Value();
			Assert.IsNotNull(tnItem.list);
			// item.tnItem.list
			Db().Deactivate(item, 2);
			// FIXME: failure 
			// Assert.isNull(tnItem.list);
			Db().Activate(item, 42);
			Db().Deactivate(item, 10);
			Assert.IsNull(tnItem.list);
		}
	}
}
