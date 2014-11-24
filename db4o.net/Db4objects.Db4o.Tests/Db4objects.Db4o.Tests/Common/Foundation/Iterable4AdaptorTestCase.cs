/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	/// <exclude></exclude>
	public class Iterable4AdaptorTestCase : ITestCase
	{
		public virtual void TestEmptyIterator()
		{
			Iterable4Adaptor adaptor = NewAdaptor(new int[] {  });
			Assert.IsFalse(adaptor.HasNext());
			Assert.IsFalse(adaptor.HasNext());
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_21(adaptor));
		}

		private sealed class _ICodeBlock_21 : ICodeBlock
		{
			public _ICodeBlock_21(Iterable4Adaptor adaptor)
			{
				this.adaptor = adaptor;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				adaptor.Next();
			}

			private readonly Iterable4Adaptor adaptor;
		}

		public virtual void TestHasNext()
		{
			int[] expected = new int[] { 1, 2, 3 };
			Iterable4Adaptor adaptor = NewAdaptor(expected);
			for (int i = 0; i < expected.Length; i++)
			{
				AssertHasNext(adaptor);
				Assert.AreEqual(expected[i], adaptor.Next());
			}
			Assert.IsFalse(adaptor.HasNext());
		}

		public virtual void TestNext()
		{
			int[] expected = new int[] { 1, 2, 3 };
			Iterable4Adaptor adaptor = NewAdaptor(expected);
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], adaptor.Next());
			}
			Assert.IsFalse(adaptor.HasNext());
		}

		private Iterable4Adaptor NewAdaptor(int[] expected)
		{
			return new Iterable4Adaptor(NewIterable(expected));
		}

		private void AssertHasNext(Iterable4Adaptor adaptor)
		{
			for (int i = 0; i < 10; ++i)
			{
				Assert.IsTrue(adaptor.HasNext());
			}
		}

		private IEnumerable NewIterable(int[] values)
		{
			Collection4 collection = new Collection4();
			collection.AddAll(IntArrays4.ToObjectArray(values));
			return collection;
		}
	}
}
