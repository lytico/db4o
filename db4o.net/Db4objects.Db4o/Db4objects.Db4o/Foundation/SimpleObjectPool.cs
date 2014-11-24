/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class SimpleObjectPool : IObjectPool
	{
		private readonly object[] _objects;

		private int _available;

		public SimpleObjectPool(object[] objects)
		{
			int length = objects.Length;
			_objects = new object[length];
			for (int i = 0; i < length; ++i)
			{
				_objects[length - i - 1] = objects[i];
			}
			_available = length;
		}

		public virtual object BorrowObject()
		{
			if (_available == 0)
			{
				throw new InvalidOperationException();
			}
			return (object)_objects[--_available];
		}

		public virtual void ReturnObject(object o)
		{
			if (_available == _objects.Length)
			{
				throw new InvalidOperationException();
			}
			_objects[_available++] = o;
		}
	}
}
