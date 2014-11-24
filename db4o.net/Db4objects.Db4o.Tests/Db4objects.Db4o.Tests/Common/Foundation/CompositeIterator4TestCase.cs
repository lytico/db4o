/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class CompositeIterator4TestCase : ITestCase
	{
		public virtual void TestWithEmptyIterators()
		{
			AssertIterator(NewIterator());
		}

		public virtual void TestReset()
		{
			CompositeIterator4 iterator = NewIterator();
			AssertIterator(iterator);
			iterator.Reset();
			AssertIterator(iterator);
		}

		private void AssertIterator(CompositeIterator4 iterator)
		{
			Iterator4Assert.AreEqual(IntArrays4.NewIterator(new int[] { 1, 2, 3, 4, 5, 6 }), 
				iterator);
		}

		private CompositeIterator4 NewIterator()
		{
			Collection4 iterators = new Collection4();
			iterators.Add(IntArrays4.NewIterator(new int[] { 1, 2, 3 }));
			iterators.Add(IntArrays4.NewIterator(new int[] {  }));
			iterators.Add(IntArrays4.NewIterator(new int[] { 4 }));
			iterators.Add(IntArrays4.NewIterator(new int[] { 5, 6 }));
			CompositeIterator4 iterator = new CompositeIterator4(iterators.GetEnumerator());
			return iterator;
		}

		public virtual void TestRecursionFree()
		{
			IList list = new ArrayList();
			IEnumerator emptyIterator = new _IEnumerator_41();
			for (int i = 0; i < 100; i++)
			{
				list.Add(emptyIterator);
			}
			IEnumerator ci = new _CompositeIterator4_59(((IEnumerator[])Sharpen.Collections.ToArray
				(list, new IEnumerator[list.Count])));
			while (ci.MoveNext())
			{
				// make .Net happy
				object current = ci.Current;
			}
		}

		private sealed class _IEnumerator_41 : IEnumerator
		{
			public _IEnumerator_41()
			{
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}

			public bool MoveNext()
			{
				return false;
			}

			public object Current
			{
				get
				{
					throw new NotSupportedException();
				}
			}
		}

		private sealed class _CompositeIterator4_59 : CompositeIterator4
		{
			public _CompositeIterator4_59(IEnumerator[] baseArg1) : base(baseArg1)
			{
				this.recursion = false;
			}

			internal bool recursion;

			public override bool MoveNext()
			{
				if (this.recursion)
				{
					Assert.Fail("Recursion in moveNext is not allowed");
				}
				this.recursion = true;
				try
				{
					return base.MoveNext();
				}
				finally
				{
					this.recursion = false;
				}
			}
		}
	}
}
