/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Nonta;

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	/// <exclude></exclude>
	public class NonTALinkedListTestCase : NonTAItemTestCaseBase
	{
		private static readonly LinkedList List = LinkedList.NewList(10);

		public static void Main(string[] args)
		{
			new NonTALinkedListTestCase().RunAll();
		}

		protected override void AssertItemValue(object obj)
		{
			Assert.AreEqual(List, ((LinkedListItem)obj).list);
		}

		protected override object CreateItem()
		{
			LinkedListItem item = new LinkedListItem();
			item.list = List;
			return item;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeactivateDepth()
		{
			LinkedListItem item = QueryItem();
			LinkedList level1 = item.list;
			LinkedList level2 = level1.NextN(1);
			LinkedList level3 = level1.NextN(2);
			LinkedList level4 = level1.NextN(3);
			LinkedList level5 = level1.NextN(4);
			Assert.IsNotNull(level1.next);
			Assert.IsNotNull(level2.next);
			Assert.IsNotNull(level3.next);
			Assert.IsNotNull(level4.next);
			Assert.IsNotNull(level5.next);
			Db().Deactivate(level1, 4);
			AssertDeactivated(level1);
			AssertDeactivated(level2);
			AssertDeactivated(level3);
			AssertDeactivated(level4);
			Assert.IsNotNull(level5.next);
		}

		private void AssertDeactivated(LinkedList list)
		{
			Assert.IsNull(list.next);
			Assert.AreEqual(0, list.value);
		}

		private LinkedListItem QueryItem()
		{
			return (LinkedListItem)RetrieveOnlyInstance();
		}
	}
}
