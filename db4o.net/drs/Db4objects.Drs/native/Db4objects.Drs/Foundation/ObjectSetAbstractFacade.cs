/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2009  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Collections;
using Db4objects.Db4o.Ext;

namespace Db4objects.Drs.Foundation
{
	public abstract class ObjectSetAbstractFacade : Db4objects.Db4o.IObjectSet
	{
		public IExtObjectSet Ext()
		{
			throw new NotImplementedException();
		}

		public virtual bool HasNext()
		{
			throw new NotImplementedException();
		}

		public virtual object Next()
		{
			throw new NotImplementedException();
		}

		public virtual void Reset()
		{
			throw new NotImplementedException();
		}

		public virtual int Size()
		{
			throw new NotImplementedException();
		}

		public int Add(object value)
		{
			throw new NotImplementedException();
		}

		public virtual bool Contains(object value)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public int IndexOf(object value)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		public void Remove(object value)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public object this[int index]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsFixedSize
		{
			get { throw new NotImplementedException(); }
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		public object SyncRoot
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsSynchronized
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}