/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Tests.Fixtures;

namespace Db4oUnit.Tests.Fixtures
{
	public class CollectionSet4 : ISet4
	{
		private ArrayList _vector = new ArrayList();

		public virtual void Add(object value)
		{
			_vector.Add(value);
		}

		public virtual bool Contains(object value)
		{
			return _vector.Contains(value);
		}

		public virtual int Size()
		{
			return _vector.Count;
		}
	}
}
