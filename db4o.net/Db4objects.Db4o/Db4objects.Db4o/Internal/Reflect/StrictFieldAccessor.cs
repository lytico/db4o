/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Reflect;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Reflect
{
	/// <since>7.7</since>
	public class StrictFieldAccessor : IFieldAccessor
	{
		public virtual object Get(IReflectField field, object onObject)
		{
			return field.Get(onObject);
		}

		public virtual void Set(IReflectField field, object onObject, object value)
		{
			field.Set(onObject, value);
		}
	}
}
