/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class IdentityHashtable4TestCase : ITestLifeCycle
	{
		private const int Key = 42;

		public class Item
		{
			public int _id;

			public Item(int id)
			{
				_id = id;
			}

			public override bool Equals(object other)
			{
				if (other == this)
				{
					return true;
				}
				if (other == null || other.GetType() != GetType())
				{
					return false;
				}
				return _id == ((IdentityHashtable4TestCase.Item)other)._id;
			}

			public override int GetHashCode()
			{
				return _id;
			}
		}

		private IMap4 _map;

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_map = new IdentityHashtable4();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}

		public virtual void TestEmpty()
		{
			Assert.IsFalse(_map.ContainsKey(new IdentityHashtable4TestCase.Item(Key)));
			Assert.IsNull(_map.Get(new IdentityHashtable4TestCase.Item(Key)));
			Assert.IsFalse(_map.Values().GetEnumerator().MoveNext());
			Assert.IsNull(_map.Remove(new IdentityHashtable4TestCase.Item(Key)));
			Assert.AreEqual(0, _map.Size());
		}

		public virtual void TestSingleEntry()
		{
			IdentityHashtable4TestCase.Item key = new IdentityHashtable4TestCase.Item(Key);
			_map.Put(key, Key);
			AssertSingleEntry(key, Key);
			_map.Put(key, Key);
			AssertSingleEntry(key, Key);
		}

		public virtual void TestDuplicateEntry()
		{
			_map.Put(new IdentityHashtable4TestCase.Item(Key), Key);
			_map.Put(new IdentityHashtable4TestCase.Item(Key), Key);
			Iterator4Assert.AreEqual(new object[] { Key, Key }, _map.Values().GetEnumerator()
				);
		}

		public virtual void TestMultipleEntries()
		{
			IdentityHashtable4TestCase.Item key1 = new IdentityHashtable4TestCase.Item(Key);
			IdentityHashtable4TestCase.Item key2 = new IdentityHashtable4TestCase.Item(Key + 
				1);
			_map.Put(key1, Key);
			_map.Put(key2, Key + 1);
			Assert.IsTrue(_map.ContainsKey(key1));
			Assert.IsTrue(_map.ContainsKey(key2));
			Assert.IsFalse(_map.ContainsKey(new IdentityHashtable4TestCase.Item(Key)));
			Assert.IsFalse(_map.ContainsKey(new IdentityHashtable4TestCase.Item(Key + 1)));
			Assert.AreEqual(Key, _map.Get(key1));
			Assert.AreEqual(Key + 1, _map.Get(key2));
			Assert.IsNull(_map.Get(new IdentityHashtable4TestCase.Item(Key)));
			Assert.IsNull(_map.Get(new IdentityHashtable4TestCase.Item(Key + 1)));
			Assert.AreEqual(2, _map.Size());
			Iterator4Assert.SameContent(new object[] { Key, Key + 1 }, _map.Values().GetEnumerator
				());
		}

		public virtual void TestRemove()
		{
			IdentityHashtable4TestCase.Item key1 = new IdentityHashtable4TestCase.Item(Key);
			IdentityHashtable4TestCase.Item key2 = new IdentityHashtable4TestCase.Item(Key + 
				1);
			_map.Put(key1, Key);
			_map.Put(key2, Key + 1);
			Assert.AreEqual(Key, _map.Remove(key1));
			Assert.IsFalse(_map.ContainsKey(key1));
			Assert.IsTrue(_map.ContainsKey(key2));
			Assert.IsNull(_map.Get(key1));
			Assert.AreEqual(Key + 1, _map.Get(key2));
			Assert.AreEqual(1, _map.Size());
			Iterator4Assert.AreEqual(new object[] { Key + 1 }, _map.Values().GetEnumerator());
		}

		public virtual void TestClear()
		{
			IdentityHashtable4TestCase.Item key1 = new IdentityHashtable4TestCase.Item(Key);
			IdentityHashtable4TestCase.Item key2 = new IdentityHashtable4TestCase.Item(Key + 
				1);
			_map.Put(key1, Key);
			_map.Put(key2, Key + 1);
			_map.Clear();
			Assert.IsFalse(_map.ContainsKey(key1));
			Assert.IsFalse(_map.ContainsKey(key2));
			Assert.IsNull(_map.Get(key1));
			Assert.IsNull(_map.Get(key2));
			Assert.AreEqual(0, _map.Size());
			Iterator4Assert.AreEqual(new object[] {  }, _map.Values().GetEnumerator());
		}

		private void AssertSingleEntry(IdentityHashtable4TestCase.Item key, int value)
		{
			Assert.IsTrue(_map.ContainsKey(key));
			Assert.IsFalse(_map.ContainsKey(new IdentityHashtable4TestCase.Item(value)));
			Assert.AreEqual(value, _map.Get(key));
			Assert.IsNull(_map.Get(new IdentityHashtable4TestCase.Item(value)));
			Assert.AreEqual(1, _map.Size());
			Iterator4Assert.AreEqual(new object[] { value }, _map.Values().GetEnumerator());
		}
	}
}
