/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>Unbounded queue.</summary>
	/// <remarks>Unbounded queue.</remarks>
	/// <exclude></exclude>
	public class NonblockingQueue : IQueue4
	{
		private List4 _insertionPoint;

		private List4 _next;

		public void Add(object obj)
		{
			List4 newNode = new List4(null, obj);
			if (_insertionPoint == null)
			{
				_next = newNode;
			}
			else
			{
				_insertionPoint._next = newNode;
			}
			_insertionPoint = newNode;
		}

		public object Next()
		{
			if (_next == null)
			{
				return null;
			}
			object ret = ((object)_next._element);
			RemoveNext();
			return ret;
		}

		private void RemoveNext()
		{
			_next = ((List4)_next._next);
			if (_next == null)
			{
				_insertionPoint = null;
			}
		}

		public virtual object NextMatching(IPredicate4 condition)
		{
			if (null == condition)
			{
				throw new ArgumentNullException();
			}
			List4 current = _next;
			List4 previous = null;
			while (null != current)
			{
				object element = ((object)current._element);
				if (condition.Match(element))
				{
					if (previous == null)
					{
						RemoveNext();
					}
					else
					{
						previous._next = ((List4)current._next);
					}
					return element;
				}
				previous = current;
				current = ((List4)current._next);
			}
			return null;
		}

		public bool HasNext()
		{
			return _next != null;
		}

		public virtual IEnumerator Iterator()
		{
			List4 origInsertionPoint = _insertionPoint;
			List4 origNext = _next;
			return new _Iterator4Impl_82(this, origInsertionPoint, origNext, _next);
		}

		private sealed class _Iterator4Impl_82 : Iterator4Impl
		{
			public _Iterator4Impl_82(NonblockingQueue _enclosing, List4 origInsertionPoint, List4
				 origNext, List4 baseArg1) : base(baseArg1)
			{
				this._enclosing = _enclosing;
				this.origInsertionPoint = origInsertionPoint;
				this.origNext = origNext;
			}

			public override bool MoveNext()
			{
				if (origInsertionPoint != this._enclosing._insertionPoint || origNext != this._enclosing
					._next)
				{
					throw new InvalidOperationException();
				}
				return base.MoveNext();
			}

			private readonly NonblockingQueue _enclosing;

			private readonly List4 origInsertionPoint;

			private readonly List4 origNext;
		}
	}
}
