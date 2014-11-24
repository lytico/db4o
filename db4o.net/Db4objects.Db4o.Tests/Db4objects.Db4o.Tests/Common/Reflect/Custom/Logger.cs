/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class Logger
	{
		public static void Log(string s)
		{
		}

		//		System.err.println(s);
		public static void LogMethodCall(object target, string methodName)
		{
			Log(target.ToString() + "." + methodName + "()");
		}

		public static void LogMethodCall(object target, string methodName, object arg)
		{
			Log(target.ToString() + "." + methodName + "(" + arg + ")");
		}

		public static void LogMethodCall(object target, string methodName, object arg1, object
			 arg2)
		{
			Log(target.ToString() + "." + methodName + "(" + arg1 + ", " + arg2 + ")");
		}
	}
}
