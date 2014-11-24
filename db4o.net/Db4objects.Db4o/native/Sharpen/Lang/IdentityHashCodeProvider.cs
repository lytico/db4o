/* Copyright (C) 2005   Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;

namespace Sharpen.Lang
{
	public class IdentityHashCodeProvider
	{
#if !CF
		public static int IdentityHashCode(object obj)
		{
			return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
		}
#else
		public static int IdentityHashCode(object obj)
		{
			if (obj == null) return 0;
			return (int) _hashMethod.Invoke(null, new object[] { obj });
		}

		private static MethodInfo _hashMethod = GetIdentityHashCodeMethod();

		private static MethodInfo GetIdentityHashCodeMethod()
		{
			Assembly assembly = typeof(object).Assembly;
			try
			{
				Type t = assembly.GetType("System.PInvoke.EE");
				return t.GetMethod(
					"Object_GetHashCode",
					BindingFlags.Public |
					BindingFlags.NonPublic |
					BindingFlags.Static);
			}
			catch (Exception e)
			{
			}
			// We may be running the CF app on .NET Framework 1.1
			// for profiling, let's give that a chance
			try
			{
				Type t = assembly.GetType(
					"System.Runtime.CompilerServices.RuntimeHelpers");
				return t.GetMethod(
					"GetHashCode",
					BindingFlags.Public |
					BindingFlags.Static);
			}
			catch
			{
			}
			return null;
		}
#endif
	}
}