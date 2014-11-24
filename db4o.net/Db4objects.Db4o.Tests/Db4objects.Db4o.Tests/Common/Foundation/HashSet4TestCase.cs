/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class HashSet4TestCase : ITestLifeCycle
	{
		private ISet4 _set;

		public virtual void TestEmpty()
		{
			AssertEmpty();
		}

		public virtual void TestSingleAdd()
		{
			object obj = new object();
			_set.Add(obj);
			Assert.IsFalse(_set.IsEmpty());
			Assert.AreEqual(1, _set.Size());
			Assert.IsTrue(_set.Contains(obj));
			Assert.IsFalse(_set.Contains(new object()));
			IEnumerator iter = _set.GetEnumerator();
			Assert.IsTrue(iter.MoveNext());
			Assert.AreEqual(obj, iter.Current);
		}

		public virtual void TestSingleRemove()
		{
			object obj = new object();
			_set.Add(obj);
			Assert.IsTrue(_set.Remove(obj));
			AssertEmpty();
		}

		public virtual void TestMultipleAddRemove()
		{
			object[] objs = new object[] { new object(), new object(), new object() };
			for (int objIndex = 0; objIndex < objs.Length; ++objIndex)
			{
				object obj = objs[objIndex];
				_set.Add(obj);
			}
			Assert.IsFalse(_set.IsEmpty());
			Assert.AreEqual(objs.Length, _set.Size());
			for (int objIndex = 0; objIndex < objs.Length; ++objIndex)
			{
				object obj = objs[objIndex];
				Assert.IsTrue(_set.Contains(obj));
			}
			Assert.IsFalse(_set.Contains(new object()));
			Iterator4Assert.SameContent(objs, _set.GetEnumerator());
		}

		public virtual void TestClear()
		{
			object[] objs = new object[] { new object(), new object(), new object() };
			for (int objIndex = 0; objIndex < objs.Length; ++objIndex)
			{
				object obj = objs[objIndex];
				_set.Add(obj);
			}
			_set.Clear();
			AssertEmpty();
		}

		private void AssertEmpty()
		{
			Assert.IsTrue(_set.IsEmpty());
			Assert.AreEqual(0, _set.Size());
			Assert.IsFalse(_set.Contains(new object()));
			Assert.IsFalse(_set.Remove(new object()));
			Assert.IsFalse(_set.GetEnumerator().MoveNext());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_set = new HashSet4();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}
	}
}
