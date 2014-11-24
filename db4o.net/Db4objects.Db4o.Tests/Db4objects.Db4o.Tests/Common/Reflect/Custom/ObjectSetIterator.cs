/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class ObjectSetIterator : IEnumerator
	{
		private readonly IObjectSet _set;

		private object _current;

		public ObjectSetIterator(IObjectSet set)
		{
			_set = set;
		}

		public virtual object Current
		{
			get
			{
				return _current;
			}
		}

		public virtual bool MoveNext()
		{
			if (_set.HasNext())
			{
				_current = _set.Next();
				return true;
			}
			return false;
		}

		public virtual void Reset()
		{
			throw new NotImplementedException();
		}
	}
}
