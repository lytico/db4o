/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Core;

namespace Db4objects.Db4o.Reflect.Core
{
	public class PlatformReflectConstructor : IReflectConstructor
	{
		private static readonly IReflectClass[] ParameterTypes = new IReflectClass[] {  };

		private Type _clazz;

		public PlatformReflectConstructor(Type clazz)
		{
			_clazz = clazz;
		}

		public virtual IReflectClass[] GetParameterTypes()
		{
			return ParameterTypes;
		}

		public virtual object NewInstance(object[] parameters)
		{
			return ReflectPlatform.CreateInstance(_clazz);
		}
	}
}
