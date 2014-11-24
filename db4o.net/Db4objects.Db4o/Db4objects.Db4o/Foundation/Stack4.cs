/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class Stack4
	{
		private List4 _tail;

		public virtual void Push(object obj)
		{
			_tail = new List4(_tail, obj);
		}

		public virtual object Peek()
		{
			if (_tail == null)
			{
				return null;
			}
			return _tail._element;
		}

		public virtual object Pop()
		{
			if (_tail == null)
			{
				throw new InvalidOperationException();
			}
			object res = _tail._element;
			_tail = ((List4)_tail._next);
			return res;
		}

		public virtual bool IsEmpty()
		{
			return _tail == null;
		}
	}
}
