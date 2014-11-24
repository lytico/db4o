/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public class GenericArray
	{
		internal GenericClass _clazz;

		internal object[] _data;

		public GenericArray(GenericClass clazz, int size)
		{
			_clazz = clazz;
			_data = new object[size];
		}

		public virtual IEnumerator Iterator()
		{
			return Iterators.Iterate(_data);
		}

		internal virtual int GetLength()
		{
			return _data.Length;
		}

		public override string ToString()
		{
			if (_clazz == null)
			{
				return base.ToString();
			}
			return _clazz.ToString(this);
		}
	}
}
