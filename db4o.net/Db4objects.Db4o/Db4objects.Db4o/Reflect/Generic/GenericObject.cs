/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public class GenericObject : IComparable
	{
		internal readonly GenericClass _class;

		private object[] _values;

		public GenericObject(GenericClass clazz)
		{
			_class = clazz;
		}

		private void EnsureValuesInitialized()
		{
			if (_values == null)
			{
				_values = new object[_class.GetFieldCount()];
			}
		}

		public virtual void Set(int index, object value)
		{
			EnsureValuesInitialized();
			_values[index] = value;
		}

		/// <param name="index"></param>
		/// <returns>the value of the field at index, based on the fields obtained GenericClass.getDeclaredFields
		/// 	</returns>
		public virtual object Get(int index)
		{
			EnsureValuesInitialized();
			return _values[index];
		}

		public override string ToString()
		{
			if (_class == null)
			{
				return base.ToString();
			}
			return _class.ToString(this);
		}

		public virtual GenericClass GetGenericClass()
		{
			return _class;
		}

		public virtual int CompareTo(object o)
		{
			return 0;
		}
	}
}
