/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class LongLinkedListTestCase : AbstractDb4oTestCase
	{
		private const int Count = 1000;

		public class LinkedList
		{
			public LongLinkedListTestCase.LinkedList _next;

			public int _depth;
		}

		private static LongLinkedListTestCase.LinkedList NewLongCircularList()
		{
			LongLinkedListTestCase.LinkedList head = new LongLinkedListTestCase.LinkedList();
			LongLinkedListTestCase.LinkedList tail = head;
			for (int i = 1; i < Count; i++)
			{
				tail._next = new LongLinkedListTestCase.LinkedList();
				tail = tail._next;
				tail._depth = i;
			}
			tail._next = head;
			return head;
		}

		/// <exception cref="System.Exception"></exception>
		public static void Main(string[] args)
		{
			new LongLinkedListTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(NewLongCircularList());
		}

		public virtual void Test()
		{
			IQuery q = NewQuery(typeof(LongLinkedListTestCase.LinkedList));
			q.Descend("_depth").Constrain(0);
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(1, objectSet.Count);
			LongLinkedListTestCase.LinkedList head = (LongLinkedListTestCase.LinkedList)objectSet
				.Next();
			Db().Activate(head, int.MaxValue);
			AssertListIsComplete(head);
			Db().Deactivate(head, int.MaxValue);
			Db().Activate(head, int.MaxValue);
			AssertListIsComplete(head);
			Db().Deactivate(head, int.MaxValue);
			Db().Refresh(head, int.MaxValue);
			AssertListIsComplete(head);
		}

		// TODO: The following produces a stack overflow. That's OK for now, peekPersisted is rarely
		//		 used and users can control behaviour with the depth parameter. 
		// 		 
		//		LinkedList peeked = (LinkedList) db().ext().peekPersisted(head, Integer.MAX_VALUE, true);
		//		assertListIsComplete(peeked);
		private void AssertListIsComplete(LongLinkedListTestCase.LinkedList head)
		{
			int count = 1;
			LongLinkedListTestCase.LinkedList tail = head._next;
			while (tail != head)
			{
				count++;
				tail = tail._next;
			}
			Assert.AreEqual(Count, count);
		}
	}
}
