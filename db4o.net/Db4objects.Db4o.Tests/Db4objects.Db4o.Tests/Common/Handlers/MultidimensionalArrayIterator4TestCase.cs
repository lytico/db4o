/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	/// <exclude></exclude>
	public class MultidimensionalArrayIterator4TestCase : AbstractDb4oTestCase
	{
		public virtual void TestEmptyArray()
		{
			AssertExhausted(Iterate(new object[0]));
		}

		public virtual void TestStringArray()
		{
			IEnumerator i = Iterate(new object[] { new object[] { "foo", "bar" }, new object[
				] { "fly" } });
			Assert.IsTrue(i.MoveNext());
			Assert.AreEqual("foo", i.Current);
			Assert.IsTrue(i.MoveNext());
			Assert.AreEqual("bar", i.Current);
			Assert.IsTrue(i.MoveNext());
			Assert.AreEqual("fly", i.Current);
			AssertExhausted(i);
		}

		public virtual void TestIntArray()
		{
			IEnumerator i = Iterate(new int[][] { new int[] { 1, 2 }, new int[] { 3 } });
			Assert.IsTrue(i.MoveNext());
			Assert.AreEqual(1, i.Current);
			Assert.IsTrue(i.MoveNext());
			Assert.AreEqual(2, i.Current);
			Assert.IsTrue(i.MoveNext());
			Assert.AreEqual(3, i.Current);
			AssertExhausted(i);
		}

		private void AssertExhausted(IEnumerator i)
		{
			Assert.IsFalse(i.MoveNext());
			Assert.Expect(typeof(IndexOutOfRangeException), new _ICodeBlock_53(i));
		}

		private sealed class _ICodeBlock_53 : ICodeBlock
		{
			public _ICodeBlock_53(IEnumerator i)
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

		private IEnumerator Iterate(object[] array)
		{
			return new MultidimensionalArrayIterator(ReflectArray(), array);
		}

		private IReflectArray ReflectArray()
		{
			return Reflector().Array();
		}
	}
}
