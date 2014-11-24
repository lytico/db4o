/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Tests.Fixtures;

namespace Db4oUnit.Tests.Fixtures
{
	public class HashtableSet4 : ISet4
	{
		internal Hashtable _table = new Hashtable();

		public virtual void Add(object value)
		{
			_table[value] = value;
		}

		public virtual bool Contains(object value)
		{
			return _table.Contains(value);
		}

		public virtual int Size()
		{
			return _table.Count;
		}
	}
}
