/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class IdentitySet4 : HashtableBase, IEnumerable
	{
		public IdentitySet4()
		{
		}

		public IdentitySet4(int size) : base(size)
		{
		}

		public virtual bool Contains(object obj)
		{
			return FindWithSameKey(new HashtableIdentityEntry(obj)) != null;
		}

		public virtual void Add(object obj)
		{
			if (null == obj)
			{
				throw new ArgumentNullException();
			}
			PutEntry(new HashtableIdentityEntry(obj));
		}

		public virtual void Remove(object obj)
		{
			if (null == obj)
			{
				throw new ArgumentNullException();
			}
			RemoveIntEntry(Runtime.IdentityHashCode(obj));
		}

		public virtual IEnumerator GetEnumerator()
		{
			return ValuesIterator();
		}
	}
}
