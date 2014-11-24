/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Db4objects.Db4o.Linq.Internals
{
	internal class OptimizeableMethodConstrains
	{
		public static bool CanBeOptimized(MethodInfo method)
		{
			return IsIListOrICollectionOfTMethod(method) || IsStringMethod(method);
		}

		public static bool IsStringMethod(MethodInfo method)
		{
			return method.DeclaringType == typeof(string);
		}

		public static bool IsIListOrICollectionOfTMethod(MethodInfo method)
		{
			Type declaringType = method.DeclaringType;
			return declaringType.IsGenericInstanceOf(typeof(ICollection<>)) 
			       || typeof(IList).IsAssignableFrom(declaringType) 
				   || declaringType == typeof(Enumerable);
		}
	}
}
