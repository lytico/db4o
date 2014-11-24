/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	public interface IReflectorConfiguration
	{
		bool TestConstructors();

		bool CallConstructor(IReflectClass clazz);
	}
}
