/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>A fixed size double ended queue with O(1) complexity for addFirst, removeFirst and removeLast operations.
	/// 	</summary>
	/// <remarks>A fixed size double ended queue with O(1) complexity for addFirst, removeFirst and removeLast operations.
	/// 	</remarks>
	public class CircularIntBuffer4 : IEnumerable
	{
		private const int Empty = -1;

		private readonly int[] _buffer;

		private int _head;

		private int _tail;

		public CircularIntBuffer4(int size)
		{
			_buffer = new int[size + 1];
		}

		public virtual int Size()
		{
			return Index(_tail - _head);
		}

		public virtual void AddFirst(int value)
		{
			int newHead = CircularIndex(_head - 1);
			if (newHead == _tail)
			{
				throw new InvalidOperationException();
			}
			_head = newHead;
			_buffer[Index(_head)] = value;
		}

		private int CircularIndex(int index)
		{
			return index % _buffer.Length;
		}

		private int Index(int i)
		{
			return i < 0 ? _buffer.Length + i : i;
		}

		public virtual int RemoveLast()
		{
			AssertNotEmpty();
			_tail = CircularIndex(_tail - 1);
			return Erase(_tail);
		}

		private void AssertNotEmpty()
		{
			if (IsEmpty())
			{
				throw new InvalidOperationException();
			}
		}

		public virtual bool IsEmpty()
		{
			return Index(_head) == Index(_tail);
		}

		public virtual bool IsFull()
		{
			return CircularIndex(_head - 1) == _tail;
		}

		public virtual int RemoveFirst()
		{
			AssertNotEmpty();
			int erased = Erase(_head);
			_head = CircularIndex(_head + 1);
			return erased;
		}

		private int Erase(int index)
		{
			int bufferIndex = Index(index);
			int erasedValue = _buffer[bufferIndex];
			_buffer[bufferIndex] = Empty;
			return erasedValue;
		}

		public virtual bool Remove(int value)
		{
			int idx = IndexOf(value);
			if (idx >= 0)
			{
				RemoveAt(idx);
				return true;
			}
			return false;
		}

		public virtual bool Contains(int value)
		{
			return IndexOf(value) >= 0;
		}

		private int IndexOf(int value)
		{
			int current = Index(_head);
			int tail = Index(_tail);
			while (current != tail)
			{
				if (value == _buffer[current])
				{
					break;
				}
				current = CircularIndex(current + 1);
			}
			return (current == tail ? -1 : current);
		}

		private void RemoveAt(int index)
		{
			if (Index(_tail - 1) == index)
			{
				RemoveLast();
				return;
			}
			if (index == Index(_head))
			{
				RemoveFirst();
				return;
			}
			int current = index;
			int tail = Index(_tail);
			while (current != tail)
			{
				int next = CircularIndex(current + 1);
				_buffer[current] = _buffer[next];
				current = next;
			}
			_tail = CircularIndex(_tail - 1);
		}

		public virtual IEnumerator GetEnumerator()
		{
			int tail = Index(_tail);
			int head = Index(_head);
			// TODO: detect concurrent modification and throw IllegalStateException
			return new _IEnumerator_123(this, head, tail);
		}

		private sealed class _IEnumerator_123 : IEnumerator
		{
			public _IEnumerator_123(CircularIntBuffer4 _enclosing, int head, int tail)
			{
				this._enclosing = _enclosing;
				this.head = head;
				this.tail = tail;
				this._index = head;
				this._current = Iterators.NoElement;
			}

			private int _index;

			private object _current;

			public object Current
			{
				get
				{
					if (this._current == Iterators.NoElement)
					{
						throw new InvalidOperationException();
					}
					return this._current;
				}
			}

			public bool MoveNext()
			{
				if (this._index == tail)
				{
					return false;
				}
				this._current = this._enclosing._buffer[this._index];
				this._index = this._enclosing.CircularIndex(this._index + 1);
				return true;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}

			private readonly CircularIntBuffer4 _enclosing;

			private readonly int head;

			private readonly int tail;
		}
	}
}
