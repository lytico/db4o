/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class CircularBufferTestCase : ITestCase
	{
		private const int BufferSize = 4;

		internal readonly CircularBuffer4 buffer = new CircularBuffer4(BufferSize);

		public virtual void TestAddFirstRemoveLast()
		{
			for (int i = 1; i < 11; ++i)
			{
				buffer.AddFirst(i);
				AssertRemoveLast(i);
			}
		}

		public virtual void TestAddFirstBounds()
		{
			FillBuffer();
			AssertIllegalAddFirst();
			buffer.RemoveLast();
			buffer.AddFirst(5);
			AssertIllegalAddFirst();
			buffer.RemoveLast();
			buffer.AddFirst(6);
		}

		private void AssertIllegalAddFirst()
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_33(this));
		}

		private sealed class _ICodeBlock_33 : ICodeBlock
		{
			public _ICodeBlock_33(CircularBufferTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.buffer.AddFirst(3);
			}

			private readonly CircularBufferTestCase _enclosing;
		}

		public virtual void TestRemoveLastBounds()
		{
			for (int i = 0; i < 3; ++i)
			{
				AssertIllegalRemoveLast();
				buffer.AddFirst(1);
				buffer.AddFirst(3);
				AssertRemoveLast(1);
				AssertRemoveLast(3);
				AssertIllegalRemoveLast();
			}
		}

		private void AssertIllegalRemoveFirst()
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_54(this));
		}

		private sealed class _ICodeBlock_54 : ICodeBlock
		{
			public _ICodeBlock_54(CircularBufferTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.buffer.RemoveFirst();
			}

			private readonly CircularBufferTestCase _enclosing;
		}

		private void AssertIllegalRemoveLast()
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_62(this));
		}

		private sealed class _ICodeBlock_62 : ICodeBlock
		{
			public _ICodeBlock_62(CircularBufferTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Run()
			{
				this._enclosing.buffer.RemoveLast();
			}

			private readonly CircularBufferTestCase _enclosing;
		}

		private void AssertRemoveLast(int value)
		{
			Assert.AreEqual(value, (int)(((int)buffer.RemoveLast())));
		}

		public virtual void TestIterator()
		{
			for (int i = 0; i < 3; ++i)
			{
				AssertIterator(new object[] {  });
				buffer.AddFirst(1);
				AssertIterator(new object[] { 1 });
				buffer.AddFirst(2);
				AssertIterator(new object[] { 2, 1 });
				buffer.RemoveLast();
				AssertIterator(new object[] { 2 });
				buffer.RemoveLast();
			}
		}

		public virtual void TestContains()
		{
			buffer.AddFirst(1);
			buffer.AddFirst(3);
			buffer.AddFirst(5);
			Assert.IsTrue(buffer.Contains(1));
			Assert.IsFalse(buffer.Contains(2));
			Assert.IsTrue(buffer.Contains(3));
			Assert.IsFalse(buffer.Contains(4));
			Assert.IsTrue(buffer.Contains(5));
		}

		public virtual void TestFullEmpty()
		{
			Assert.IsTrue(buffer.IsEmpty());
			Assert.IsFalse(buffer.IsFull());
			buffer.AddFirst(1);
			Assert.IsFalse(buffer.IsEmpty());
			Assert.IsFalse(buffer.IsFull());
			buffer.AddFirst(2);
			buffer.AddFirst(3);
			buffer.AddFirst(4);
			Assert.IsFalse(buffer.IsEmpty());
			Assert.IsTrue(buffer.IsFull());
			buffer.RemoveLast();
			Assert.IsFalse(buffer.IsEmpty());
			Assert.IsFalse(buffer.IsFull());
		}

		public virtual void TestSize()
		{
			for (int i = 0; i < 3; ++i)
			{
				AssertSize(0);
				for (int j = 0; j < BufferSize; ++j)
				{
					buffer.AddFirst(j);
					AssertSize(j + 1);
				}
				for (int j = 0; j < BufferSize; ++j)
				{
					buffer.RemoveLast();
					AssertSize(BufferSize - j - 1);
				}
			}
		}

		private void AssertSize(int expected)
		{
			Assert.AreEqual(expected, buffer.Size());
		}

		private void AssertIterator(object[] expected)
		{
			Iterator4Assert.AreEqual(expected, buffer.GetEnumerator());
		}

		public virtual void TestRemove()
		{
			AssertIllegalRemove(42);
			buffer.AddFirst(1);
			AssertRemove(1);
			FillBuffer();
			AssertRemovals(new int[] { 1, 2, 3, 4 });
			FillBuffer();
			AssertRemovals(new int[] { 2, 3, 4, 1 });
			FillBuffer();
			AssertRemovals(new int[] { 3, 2, 4, 1 });
			FillBuffer();
			AssertRemovals(new int[] { 4, 3, 2, 1 });
			FillBuffer();
			AssertRemovals(new int[] { 4, 1, 2, 3 });
			FillBuffer();
			AssertRemoveLast(1);
			AssertRemoveLast(2);
			AssertRemoveLast(3);
			AssertRemoveLast(4);
		}

		private void AssertRemovals(int[] indexes)
		{
			for (int iIndex = 0; iIndex < indexes.Length; ++iIndex)
			{
				int i = indexes[iIndex];
				AssertRemove(i);
			}
			AssertIllegalRemoveLast();
			AssertIllegalRemoveFirst();
		}

		private void AssertRemove(int value)
		{
			Assert.IsTrue(buffer.Remove(value));
			AssertIllegalRemove(value);
		}

		private void AssertIllegalRemove(int value)
		{
			Assert.IsFalse(buffer.Remove(value));
		}

		private void FillBuffer()
		{
			for (int i = 1; i <= BufferSize; i++)
			{
				buffer.AddFirst(i);
			}
		}
	}
}
