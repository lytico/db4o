/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Net;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.native.Db4objects.Db4o.Internal
{
	public class GenericTypeHandlerPredicate : ITypeHandlerPredicate
	{
		private readonly Type _genericType;

		public GenericTypeHandlerPredicate(Type genericType)
		{
			_genericType = genericType;
		}

		public bool Match(IReflectClass classReflector)
		{
			Type type = NetReflector.ToNative(classReflector);
			if (type == null)
			{
				return false;
			}
			if (!type.IsGenericType)
			{
				return false;
			}
			return type.GetGenericTypeDefinition() == _genericType;
		}
	}
}
