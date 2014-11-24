/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class NonblockingQueueTestCase : Queue4TestCaseBase
	{
		public virtual void TestIterator()
		{
			IQueue4 queue = new NonblockingQueue();
			string[] data = new string[] { "a", "b", "c", "d" };
			for (int idx = 0; idx < data.Length; idx++)
			{
				AssertIterator(queue, data, idx);
				queue.Add(data[idx]);
				AssertIterator(queue, data, idx + 1);
			}
		}

		public virtual void TestIteratorThrowsOnConcurrentModification()
		{
			object[] elements = new object[] { "foo", "bar" };
			IQueue4 queue = NewQueue(elements);
			IEnumerator iterator = queue.Iterator();
			Iterator4Assert.AssertNext("foo", iterator);
			queue.Add("baz");
			Assert.AreEqual("foo", iterator.Current, "accessing current element should be harmless"
				);
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_31(iterator));
		}

		private sealed class _ICodeBlock_31 : ICodeBlock
		{
			public _ICodeBlock_31(IEnumerator iterator)
			{
				this.iterator = iterator;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				iterator.MoveNext();
			}

			private readonly IEnumerator iterator;
		}

		public virtual void TestNextMatchingFailure()
		{
			object[] elements = new object[] { "foo", "bar" };
			IQueue4 queue = NewQueue(elements);
			Assert.IsNull(queue.NextMatching(new _IPredicate4_42()));
			AssertNext(elements, queue);
		}

		private sealed class _IPredicate4_42 : IPredicate4
		{
			public _IPredicate4_42()
			{
			}

			public bool Match(object candidate)
			{
				return false;
			}
		}

		public virtual void TestNextMatchingOnEmptyQueue()
		{
			object[] empty = new object[0];
			AssertNextMatching(empty, null, empty);
		}

		public virtual void TestNextMatching()
		{
			object first = "42";
			object second = 42;
			object last = System.Convert.ToSingle(42.0);
			object[] elements = new object[] { first, second, last };
			AssertNextMatching(new object[] { first, last }, second, elements);
			AssertNextMatching(new object[] { second, last }, first, elements);
			AssertNextMatching(new object[] { first, second }, last, elements);
		}

		private void AssertNextMatching(object[] expectedAfterRemoval, object removedElement
			, object[] originalElements)
		{
			IQueue4 queue = NewQueue(originalElements);
			Assert.AreEqual(removedElement, queue.NextMatching(new _IPredicate4_73(removedElement
				)));
			AssertNext(expectedAfterRemoval, queue);
		}

		private sealed class _IPredicate4_73 : IPredicate4
		{
			public _IPredicate4_73(object removedElement)
			{
				this.removedElement = removedElement;
			}

			public bool Match(object candidate)
			{
				return removedElement == candidate;
			}

			private readonly object removedElement;
		}

		private void AssertNext(object[] expected, IQueue4 queue)
		{
			for (int i = 0; i < expected.Length; i++)
			{
				object @object = expected[i];
				Assert.IsTrue(queue.HasNext(), "Expecting '" + @object + "'");
				Assert.AreSame(@object, queue.Next());
			}
			Assert.IsFalse(queue.HasNext());
		}

		private IQueue4 NewQueue(object[] items)
		{
			NonblockingQueue queue = new NonblockingQueue();
			for (int i = 0; i < items.Length; i++)
			{
				queue.Add(items[i]);
			}
			return queue;
		}

		public virtual void TestNext()
		{
			IQueue4 queue = new NonblockingQueue();
			string[] data = new string[] { "a", "b", "c" };
			queue.Add(data[0]);
			Assert.AreSame(data[0], queue.Next());
			queue.Add(data[1]);
			queue.Add(data[2]);
			AssertNext(new object[] { data[1], data[2] }, queue);
		}
	}
}
