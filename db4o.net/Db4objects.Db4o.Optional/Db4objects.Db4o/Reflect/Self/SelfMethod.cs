/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect.Self
{
	public class SelfMethod : IReflectMethod
	{
		public virtual object Invoke(object onObject, object[] parameters)
		{
			return null;
		}

		public virtual IReflectClass GetReturnType()
		{
			return null;
		}
	}
}
