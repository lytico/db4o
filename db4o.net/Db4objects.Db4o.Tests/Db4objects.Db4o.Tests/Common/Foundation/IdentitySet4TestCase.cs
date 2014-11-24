/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class IdentitySet4TestCase : ITestCase
	{
		public class Item
		{
			internal int _id;

			public Item(int id)
			{
				_id = id;
			}

			public override int GetHashCode()
			{
				return _id;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is IdentitySet4TestCase.Item))
				{
					return false;
				}
				IdentitySet4TestCase.Item other = (IdentitySet4TestCase.Item)obj;
				return _id == other._id;
			}
		}

		public virtual void TestByIdentity()
		{
			IdentitySet4 table = new IdentitySet4(2);
			IdentitySet4TestCase.Item item1 = new IdentitySet4TestCase.Item(1);
			Assert.IsFalse(table.Contains(item1));
			table.Add(item1);
			Assert.IsTrue(table.Contains(item1));
			IdentitySet4TestCase.Item item2 = new IdentitySet4TestCase.Item(2);
			Assert.IsFalse(table.Contains(item2));
			table.Add(item2);
			Assert.IsTrue(table.Contains(item2));
			Assert.AreEqual(2, table.Size());
			int size = 0;
			IEnumerator i = table.GetEnumerator();
			while (i.MoveNext())
			{
				size++;
			}
			Assert.AreEqual(2, size);
		}

		public virtual void TestRemove()
		{
			IdentitySet4 set = new IdentitySet4();
			object obj = new object();
			set.Add(obj);
			Assert.IsTrue(set.Contains(obj));
			set.Remove(obj);
			Assert.IsFalse(set.Contains(obj));
		}

		public virtual void TestIterator()
		{
			IdentitySet4 set = new IdentitySet4();
			object o1 = new object();
			object o2 = new object();
			set.Add(o1);
			set.Add(o2);
			Iterator4Assert.SameContent(Iterators.Iterate(new object[] { o1, o2 }), set.GetEnumerator
				());
		}
	}
}
