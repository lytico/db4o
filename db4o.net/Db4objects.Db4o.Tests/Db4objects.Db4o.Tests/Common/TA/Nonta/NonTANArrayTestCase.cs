/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Nonta;

namespace Db4objects.Db4o.Tests.Common.TA.Nonta
{
	/// <exclude></exclude>
	public class NonTANArrayTestCase : NonTAItemTestCaseBase
	{
		private static readonly int[][] Ints1 = new int[][] { new int[] { 1, 2, 3 }, new 
			int[] { 4, 5, 6 } };

		private static readonly int[][] Ints2 = new int[][] { new int[] { 4, 5, 6 }, new 
			int[] { 7, 8, 9 } };

		private static readonly LinkedList[][] List1 = new LinkedList[][] { new LinkedList
			[] { LinkedList.NewList(5) }, new LinkedList[] { LinkedList.NewList(5) } };

		private static readonly LinkedList[][] List2 = new LinkedList[][] { new LinkedList
			[] { LinkedList.NewList(5) }, new LinkedList[] { LinkedList.NewList(5) } };

		public static void Main(string[] args)
		{
			new NonTANArrayTestCase().RunAll();
		}

		protected override void AssertItemValue(object obj)
		{
			NArrayItem item = (NArrayItem)obj;
			JaggedArrayAssert.AreEqual(Ints1, item.Value());
			JaggedArrayAssert.AreEqual(Ints2, (int[][])item.Object());
			JaggedArrayAssert.AreEqual(List1, item.Lists());
			JaggedArrayAssert.AreEqual(List2, (LinkedList[][])item.ListsObject());
		}

		protected override object CreateItem()
		{
			NArrayItem item = new NArrayItem();
			item.value = Ints1;
			item.obj = Ints2;
			item.lists = List1;
			item.listsObject = List2;
			return item;
		}
	}
}
