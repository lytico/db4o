/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class Hashtable4TestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(Hashtable4TestCase)).Run();
		}

		public virtual void TestClear()
		{
			Hashtable4 table = new Hashtable4();
			for (int i = 0; i < 2; ++i)
			{
				table.Clear();
				Assert.AreEqual(0, table.Size());
				table.Put("foo", "bar");
				Assert.AreEqual(1, table.Size());
				AssertIterator(table, new object[] { "foo" });
			}
		}

		public virtual void TestToString()
		{
			Hashtable4 table = new Hashtable4();
			table.Put("foo", "bar");
			table.Put("bar", "baz");
			Assert.AreEqual(Iterators.Join(table.Iterator(), "{", "}", ", "), table.ToString(
				));
		}

		public virtual void TestContainsKey()
		{
			Hashtable4 table = new Hashtable4();
			Assert.IsFalse(table.ContainsKey(null));
			Assert.IsFalse(table.ContainsKey("foo"));
			table.Put("foo", null);
			Assert.IsTrue(table.ContainsKey("foo"));
			table.Put("bar", "baz");
			Assert.IsTrue(table.ContainsKey("bar"));
			Assert.IsFalse(table.ContainsKey("baz"));
			Assert.IsTrue(table.ContainsKey("foo"));
			table.Remove("foo");
			Assert.IsTrue(table.ContainsKey("bar"));
			Assert.IsFalse(table.ContainsKey("foo"));
		}

		public virtual void TestByteArrayKeys()
		{
			byte[] key1 = new byte[] { 1, 2, 3 };
			byte[] key2 = new byte[] { 3, 2, 1 };
			byte[] key3 = new byte[] { 3, 2, 1 };
			// same values as key2
			Hashtable4 table = new Hashtable4(2);
			table.Put(key1, "foo");
			table.Put(key2, "bar");
			Assert.AreEqual("foo", table.Get(key1));
			Assert.AreEqual("bar", table.Get(key2));
			Assert.AreEqual(2, CountKeys(table));
			Assert.AreEqual(2, table.Size());
			table.Put(key3, "baz");
			Assert.AreEqual("foo", table.Get(key1));
			Assert.AreEqual("baz", table.Get(key2));
			Assert.AreEqual(2, CountKeys(table));
			Assert.AreEqual(2, table.Size());
			Assert.AreEqual("baz", table.Remove(key2));
			Assert.AreEqual(1, CountKeys(table));
			Assert.AreEqual(1, table.Size());
			Assert.AreEqual("foo", table.Remove(key1));
			Assert.AreEqual(0, CountKeys(table));
			Assert.AreEqual(0, table.Size());
		}

		public virtual void TestIterator()
		{
			AssertIsIteratable(new object[0]);
			AssertIsIteratable(new object[] { "one" });
			AssertIsIteratable(new object[] { 1, 3, 2 });
			AssertIsIteratable(new object[] { "one", "three", "two" });
			AssertIsIteratable(new object[] { new Hashtable4TestCase.Key(1), new Hashtable4TestCase.Key
				(3), new Hashtable4TestCase.Key(2) });
		}

		public virtual void TestSameKeyTwice()
		{
			int key = 1;
			Hashtable4 table = new Hashtable4();
			table.Put(key, "foo");
			table.Put(key, "bar");
			Assert.AreEqual("bar", table.Get(key));
			Assert.AreEqual(1, CountKeys(table));
		}

		public virtual void TestSameHashCodeIterator()
		{
			Hashtable4TestCase.Key[] keys = CreateKeys(1, 5);
			AssertIsIteratable(keys);
		}

		private Hashtable4TestCase.Key[] CreateKeys(int begin, int end)
		{
			int factor = 10;
			int count = (end - begin);
			Hashtable4TestCase.Key[] keys = new Hashtable4TestCase.Key[count * factor];
			for (int i = 0; i < count; ++i)
			{
				int baseIndex = i * factor;
				for (int j = 0; j < factor; ++j)
				{
					keys[baseIndex + j] = new Hashtable4TestCase.Key(begin + i);
				}
			}
			return keys;
		}

		public virtual void TestDifferentKeysSameHashCode()
		{
			Hashtable4TestCase.Key key1 = new Hashtable4TestCase.Key(1);
			Hashtable4TestCase.Key key2 = new Hashtable4TestCase.Key(1);
			Hashtable4TestCase.Key key3 = new Hashtable4TestCase.Key(2);
			Hashtable4 table = new Hashtable4(2);
			table.Put(key1, "foo");
			table.Put(key2, "bar");
			Assert.AreEqual("foo", table.Get(key1));
			Assert.AreEqual("bar", table.Get(key2));
			Assert.AreEqual(2, CountKeys(table));
			table.Put(key2, "baz");
			Assert.AreEqual("foo", table.Get(key1));
			Assert.AreEqual("baz", table.Get(key2));
			Assert.AreEqual(2, CountKeys(table));
			table.Put(key1, "spam");
			Assert.AreEqual("spam", table.Get(key1));
			Assert.AreEqual("baz", table.Get(key2));
			Assert.AreEqual(2, CountKeys(table));
			table.Put(key3, "eggs");
			Assert.AreEqual("spam", table.Get(key1));
			Assert.AreEqual("baz", table.Get(key2));
			Assert.AreEqual("eggs", table.Get(key3));
			Assert.AreEqual(3, CountKeys(table));
			table.Put(key2, "mice");
			Assert.AreEqual("spam", table.Get(key1));
			Assert.AreEqual("mice", table.Get(key2));
			Assert.AreEqual("eggs", table.Get(key3));
			Assert.AreEqual(3, CountKeys(table));
		}

		internal class KeyCount
		{
			public int keys;
		}

		private int CountKeys(Hashtable4 table)
		{
			int count = 0;
			IEnumerator i = table.Iterator();
			while (i.MoveNext())
			{
				count++;
			}
			return count;
		}

		public virtual void AssertIsIteratable(object[] keys)
		{
			Hashtable4 table = TableFromKeys(keys);
			AssertIterator(table, keys);
		}

		private void AssertIterator(Hashtable4 table, object[] keys)
		{
			IEnumerator iter = table.Iterator();
			Collection4 expected = new Collection4(keys);
			while (iter.MoveNext())
			{
				IEntry4 entry = (IEntry4)iter.Current;
				bool removedOK = expected.Remove(entry.Key());
				Assert.IsTrue(removedOK);
			}
			Assert.IsTrue(expected.IsEmpty(), expected.ToString());
		}

		private Hashtable4 TableFromKeys(object[] keys)
		{
			Hashtable4 ht = new Hashtable4();
			for (int i = 0; i < keys.Length; i++)
			{
				ht.Put(keys[i], keys[i]);
			}
			return ht;
		}

		internal class Key
		{
			private int _hashCode;

			public Key(int hashCode)
			{
				_hashCode = hashCode;
			}

			public override int GetHashCode()
			{
				return _hashCode;
			}
		}
	}
}
