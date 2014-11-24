/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class Collection4TestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(Collection4TestCase)).Run();
		}

		public virtual void TestRemoveAll()
		{
			string[] originalElements = new string[] { "foo", "bar", "baz" };
			Collection4 c = NewCollection(originalElements);
			c.RemoveAll(NewCollection(new string[0]));
			AssertCollection(originalElements, c);
			c.RemoveAll(NewCollection(new string[] { "baz", "bar", "zeng" }));
			AssertCollection(new string[] { "foo" }, c);
			c.RemoveAll(NewCollection(originalElements));
			AssertCollection(new string[0], c);
		}

		public virtual void TestContains()
		{
			object a = new object();
			Collection4 c = new Collection4();
			c.Add(new object());
			Assert.IsFalse(c.Contains(a));
			c.Add(a);
			Assert.IsTrue(c.Contains(a));
			c.Remove(a);
			Assert.IsFalse(c.Contains(a));
		}

		private class Item
		{
			public int id;

			public override int GetHashCode()
			{
				int prime = 31;
				int result = 1;
				result = prime * result + id;
				return result;
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null)
				{
					return false;
				}
				if (GetType() != obj.GetType())
				{
					return false;
				}
				Collection4TestCase.Item other = (Collection4TestCase.Item)obj;
				if (id != other.id)
				{
					return false;
				}
				return true;
			}

			public Item(int id)
			{
				this.id = id;
			}
		}

		public virtual void TestContainsAll()
		{
			Collection4TestCase.Item a = new Collection4TestCase.Item(42);
			Collection4TestCase.Item b = new Collection4TestCase.Item(a.id + 1);
			Collection4TestCase.Item c = new Collection4TestCase.Item(b.id + 1);
			Collection4TestCase.Item a_ = new Collection4TestCase.Item(a.id);
			Collection4 needle = new Collection4();
			Collection4 haystack = new Collection4();
			haystack.Add(a);
			needle.Add(a);
			needle.Add(b);
			Assert.IsFalse(haystack.ContainsAll(needle));
			needle.Remove(b);
			Assert.IsTrue(haystack.ContainsAll(needle));
			needle.Add(b);
			haystack.Add(b);
			Assert.IsTrue(haystack.ContainsAll(needle));
			needle.Add(a_);
			Assert.IsTrue(haystack.ContainsAll(needle));
			needle.Add(c);
			Assert.IsFalse(haystack.ContainsAll(needle));
			needle.Clear();
			Assert.IsTrue(haystack.ContainsAll(needle));
			haystack.Clear();
			Assert.IsTrue(haystack.ContainsAll(needle));
		}

		public virtual void TestReplace()
		{
			Collection4 c = new Collection4();
			c.Replace("one", "two");
			c.Add("one");
			c.Add("two");
			c.Add("three");
			c.Replace("two", "two.half");
			AssertCollection(new string[] { "one", "two.half", "three" }, c);
			c.Replace("two.half", "one");
			c.Replace("one", "half");
			AssertCollection(new string[] { "half", "one", "three" }, c);
		}

		public virtual void TestNulls()
		{
			Collection4 c = new Collection4();
			c.Add("one");
			AssertNotContainsNull(c);
			c.Add(null);
			AssertContainsNull(c);
			AssertCollection(new string[] { "one", null }, c);
			c.Prepend(null);
			AssertCollection(new string[] { null, "one", null }, c);
			c.Prepend("zero");
			c.Add("two");
			AssertCollection(new string[] { "zero", null, "one", null, "two" }, c);
			AssertContainsNull(c);
			c.Remove(null);
			AssertCollection(new string[] { "zero", "one", null, "two" }, c);
			c.Remove(null);
			AssertNotContainsNull(c);
			AssertCollection(new string[] { "zero", "one", "two" }, c);
			c.Remove(null);
			AssertCollection(new string[] { "zero", "one", "two" }, c);
		}

		public virtual void TestGetByIndex()
		{
			Collection4 c = new Collection4();
			c.Add("one");
			c.Add("two");
			Assert.AreEqual("one", c.Get(0));
			Assert.AreEqual("two", c.Get(1));
			AssertIllegalIndex(c, -1);
			AssertIllegalIndex(c, 2);
		}

		public virtual void TestIndexOf()
		{
			Collection4 c = new Collection4();
			Assert.AreEqual(-1, c.IndexOf("notInCollection"));
			c.Add("one");
			Assert.AreEqual(-1, c.IndexOf("notInCollection"));
			Assert.AreEqual(0, c.IndexOf("one"));
			c.Add("two");
			c.Add("three");
			Assert.AreEqual(0, c.IndexOf("one"));
			Assert.AreEqual(1, c.IndexOf("two"));
			Assert.AreEqual(2, c.IndexOf("three"));
			Assert.AreEqual(-1, c.IndexOf("notInCollection"));
		}

		private void AssertIllegalIndex(Collection4 c, int index)
		{
			Assert.Expect(typeof(ArgumentException), new _ICodeBlock_170(c, index));
		}

		private sealed class _ICodeBlock_170 : ICodeBlock
		{
			public _ICodeBlock_170(Collection4 c, int index)
			{
				this.c = c;
				this.index = index;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				c.Get(index);
			}

			private readonly Collection4 c;

			private readonly int index;
		}

		public virtual void TestPrepend()
		{
			Collection4 c = new Collection4();
			c.Prepend("foo");
			AssertCollection(new string[] { "foo" }, c);
			c.Add("bar");
			AssertCollection(new string[] { "foo", "bar" }, c);
			c.Prepend("baz");
			AssertCollection(new string[] { "baz", "foo", "bar" }, c);
			c.Prepend("gazonk");
			AssertCollection(new string[] { "gazonk", "baz", "foo", "bar" }, c);
		}

		public virtual void TestCopyConstructor()
		{
			string[] expected = new string[] { "1", "2", "3" };
			Collection4 c = NewCollection(expected);
			AssertCollection(expected, new Collection4(c));
		}

		public virtual void TestInvalidIteratorException()
		{
			Collection4 c = NewCollection(new string[] { "1", "2" });
			IEnumerator i = c.GetEnumerator();
			Assert.IsTrue(i.MoveNext());
			c.Add("3");
			Assert.Expect(typeof(InvalidIteratorException), new _ICodeBlock_200(i));
		}

		private sealed class _ICodeBlock_200 : ICodeBlock
		{
			public _ICodeBlock_200(IEnumerator i)
			{
				this.i = i;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Sharpen.Runtime.Out.WriteLine(i.Current);
			}

			private readonly IEnumerator i;
		}

		public virtual void TestRemove()
		{
			Collection4 c = NewCollection(new string[] { "1", "2", "3", "4" });
			c.Remove("3");
			AssertCollection(new string[] { "1", "2", "4" }, c);
			c.Remove("4");
			AssertCollection(new string[] { "1", "2" }, c);
			c.Add("5");
			AssertCollection(new string[] { "1", "2", "5" }, c);
			c.Remove("1");
			AssertCollection(new string[] { "2", "5" }, c);
			c.Remove("2");
			c.Remove("5");
			AssertCollection(new string[] {  }, c);
			c.Add("6");
			AssertCollection(new string[] { "6" }, c);
		}

		private void AssertCollection(string[] expected, Collection4 c)
		{
			Assert.AreEqual(expected.Length, c.Size());
			Iterator4Assert.AreEqual(expected, c.GetEnumerator());
		}

		private void AssertContainsNull(Collection4 c)
		{
			Assert.IsTrue(c.Contains(null));
			Assert.IsNull(c.Get(null));
			int size = c.Size();
			c.Ensure(null);
			Assert.AreEqual(size, c.Size());
		}

		private void AssertNotContainsNull(Collection4 c)
		{
			Assert.IsFalse(c.Contains(null));
			Assert.IsNull(c.Get(null));
			int size = c.Size();
			c.Ensure(null);
			Assert.AreEqual(size + 1, c.Size());
			c.Remove(null);
			Assert.AreEqual(size, c.Size());
		}

		public virtual void TestIterator()
		{
			string[] expected = new string[] { "1", "2", "3" };
			Collection4 c = NewCollection(expected);
			Iterator4Assert.AreEqual(expected, c.GetEnumerator());
		}

		private Collection4 NewCollection(string[] expected)
		{
			Collection4 c = new Collection4();
			c.AddAll(expected);
			return c;
		}

		public virtual void TestToString()
		{
			Collection4 c = new Collection4();
			Assert.AreEqual("[]", c.ToString());
			c.Add("foo");
			Assert.AreEqual("[foo]", c.ToString());
			c.Add("bar");
			Assert.AreEqual("[foo, bar]", c.ToString());
		}
	}
}
