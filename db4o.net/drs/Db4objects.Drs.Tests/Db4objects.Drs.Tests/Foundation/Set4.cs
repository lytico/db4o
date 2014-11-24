/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Drs.Tests.Foundation
{
	public class Set4 : IEnumerable
	{
		public static readonly Db4objects.Drs.Tests.Foundation.Set4 EmptySet = new Db4objects.Drs.Tests.Foundation.Set4
			(0);

		private readonly Hashtable4 _table;

		public Set4()
		{
			_table = new Hashtable4();
		}

		public Set4(int size)
		{
			_table = new Hashtable4(size);
		}

		public Set4(IEnumerable keys) : this()
		{
			AddAll(keys);
		}

		public virtual void Add(object element)
		{
			_table.Put(element, element);
		}

		public virtual void AddAll(IEnumerable other)
		{
			IEnumerator i = other.GetEnumerator();
			while (i.MoveNext())
			{
				Add(i.Current);
			}
		}

		public virtual bool IsEmpty()
		{
			return _table.Size() == 0;
		}

		public virtual int Size()
		{
			return _table.Size();
		}

		public virtual bool Contains(object element)
		{
			return _table.Get(element) != null;
		}

		public virtual bool ContainsAll(Db4objects.Drs.Tests.Foundation.Set4 other)
		{
			return _table.ContainsAllKeys(other);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _table.Keys();
		}

		public override string ToString()
		{
			return Iterators.Join(GetEnumerator(), "[", "]", ", ");
		}
	}
}
