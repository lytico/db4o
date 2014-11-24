/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class FlatteningIterator : CompositeIterator4
	{
		private class IteratorStack
		{
			public readonly IEnumerator iterator;

			public readonly FlatteningIterator.IteratorStack next;

			public IteratorStack(IEnumerator iterator_, FlatteningIterator.IteratorStack next_
				)
			{
				iterator = iterator_;
				next = next_;
			}
		}

		private FlatteningIterator.IteratorStack _stack;

		public FlatteningIterator(IEnumerator iterators) : base(iterators)
		{
		}

		public override bool MoveNext()
		{
			if (null == _currentIterator)
			{
				if (null == _stack)
				{
					_currentIterator = _iterators;
				}
				else
				{
					_currentIterator = Pop();
				}
			}
			if (!_currentIterator.MoveNext())
			{
				if (_currentIterator == _iterators)
				{
					return false;
				}
				_currentIterator = null;
				return MoveNext();
			}
			object current = _currentIterator.Current;
			if (current is IEnumerator)
			{
				Push(_currentIterator);
				_currentIterator = NextIterator(current);
				return MoveNext();
			}
			return true;
		}

		private void Push(IEnumerator currentIterator)
		{
			_stack = new FlatteningIterator.IteratorStack(currentIterator, _stack);
		}

		private IEnumerator Pop()
		{
			IEnumerator iterator = _stack.iterator;
			_stack = _stack.next;
			return iterator;
		}
	}
}
