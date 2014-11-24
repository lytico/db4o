/* Copyright (C) 2007 - 2010  Versant Inc.  http://www.db4o.com */

using System.Reflection;

namespace Db4objects.Db4o.Linq.CodeAnalysis
{
	internal static class MethodAnalyser
	{
		public static IMethodAnalyser FromMethod(MethodInfo method)
		{
#if CF || SILVERLIGHT
			return CecilMethodAnalyser.FromMethod(method);
#else
			return ReflectionMethodAnalyser.FromMethod(method);
#endif
		}
	}
}
