/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class PeekPersistedTestCase : AbstractDb4oTestCase, IOptOutStackOverflowIssue
	{
		public sealed class Item
		{
			public string name;

			public PeekPersistedTestCase.Item child;

			public Item()
			{
			}

			public Item(string name, PeekPersistedTestCase.Item child)
			{
				this.name = name;
				this.child = child;
			}

			public override string ToString()
			{
				return "Item(" + name + ", " + child + ")";
			}
		}

		protected override void Store()
		{
			PeekPersistedTestCase.Item root = new PeekPersistedTestCase.Item("1", null);
			PeekPersistedTestCase.Item current = root;
			for (int i = 2; i < 11; i++)
			{
				current.child = new PeekPersistedTestCase.Item(string.Empty + i, null);
				current = current.child;
			}
			Store(root);
		}

		public virtual void Test()
		{
			PeekPersistedTestCase.Item root = QueryRoot();
			for (int i = 0; i < 10; i++)
			{
				Peek(root, i);
			}
		}

		private PeekPersistedTestCase.Item QueryRoot()
		{
			IQuery q = NewQuery(typeof(PeekPersistedTestCase.Item));
			q.Descend("name").Constrain("1");
			IObjectSet objectSet = q.Execute();
			return (PeekPersistedTestCase.Item)objectSet.Next();
		}

		private void Peek(PeekPersistedTestCase.Item original, int depth)
		{
			PeekPersistedTestCase.Item peeked = (PeekPersistedTestCase.Item)((PeekPersistedTestCase.Item
				)Db().PeekPersisted(original, depth, true));
			for (int i = 0; i <= depth; i++)
			{
				Assert.IsNotNull(peeked, "Failed to peek at child " + i + " at depth " + depth);
				Assert.IsFalse(Db().IsStored(peeked));
				peeked = peeked.child;
			}
			Assert.IsNull(peeked);
		}
	}
}
