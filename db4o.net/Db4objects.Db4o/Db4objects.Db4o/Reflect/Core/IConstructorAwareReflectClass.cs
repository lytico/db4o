/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Core;

namespace Db4objects.Db4o.Reflect.Core
{
	public interface IConstructorAwareReflectClass : IReflectClass
	{
		IReflectConstructor GetSerializableConstructor();
	}
}
