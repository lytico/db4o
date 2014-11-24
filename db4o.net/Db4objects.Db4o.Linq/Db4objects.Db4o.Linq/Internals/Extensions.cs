/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Db4objects.Db4o.Linq.Internals
{
	internal static class Extensions
	{
		public static Type[] GetParameterTypes(this MethodBase self)
		{
			return self.GetParameters().Select(p => p.ParameterType).ToArray();
		}

		public static Type MakeGenericTypeFrom(this Type self, Type type)
		{
			return self.MakeGenericType(type.GetGenericArguments());
		}

		public static Type GetFirstGenericArgument(this Type self)
		{
			return self.GetGenericArguments()[0];
		}

		public static bool IsGenericInstanceOf(this Type self, Type type)
		{
			return self.IsGenericType && self.GetGenericTypeDefinition() == type;
		}

		public static MethodInfo MakeGenericMethodFrom(this MethodInfo self, MethodInfo method)
		{
			return self.MakeGenericMethod(method.GetGenericArguments());
		}

		public static bool IsExtension(this MethodInfo self)
		{
			return self.GetCustomAttributes(typeof(ExtensionAttribute), false).Length > 0;
		}
	}
}
