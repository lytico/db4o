/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class HashSet4 : ISet4
	{
		private Hashtable4 _map;

		public HashSet4() : this(1)
		{
		}

		public HashSet4(int count)
		{
			_map = new Hashtable4(count);
		}

		public virtual bool Add(object obj)
		{
			if (_map.ContainsKey(obj))
			{
				return false;
			}
			_map.Put(obj, obj);
			return true;
		}

		public virtual void Clear()
		{
			_map.Clear();
		}

		public virtual bool Contains(object obj)
		{
			return _map.ContainsKey(obj);
		}

		public virtual bool IsEmpty()
		{
			return _map.Size() == 0;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _map.Values().GetEnumerator();
		}

		public virtual bool Remove(object obj)
		{
			return _map.Remove(obj) != null;
		}

		public virtual int Size()
		{
			return _map.Size();
		}

		public override string ToString()
		{
			return Iterators.Join(_map.Keys(), "{", "}", ", ");
		}
	}
}
