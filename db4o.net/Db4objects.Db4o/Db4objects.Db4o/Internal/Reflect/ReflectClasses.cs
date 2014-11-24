/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Reflect
{
	public class ReflectClasses
	{
		public static bool AreEqual(Type expected, IReflectClass actual)
		{
			return actual.Reflector().ForClass(expected) == actual;
		}
	}
}
