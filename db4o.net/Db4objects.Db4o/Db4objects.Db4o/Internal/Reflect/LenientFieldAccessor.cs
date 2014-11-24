/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Reflect;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Reflect
{
	/// <since>7.7</since>
	public class LenientFieldAccessor : IFieldAccessor
	{
		public virtual object Get(IReflectField field, object onObject)
		{
			try
			{
				return field.Get(onObject);
			}
			catch (Db4oException)
			{
				return null;
			}
		}

		public virtual void Set(IReflectField field, object onObject, object value)
		{
			try
			{
				field.Set(onObject, value);
			}
			catch (Db4oException)
			{
			}
		}
	}
}
