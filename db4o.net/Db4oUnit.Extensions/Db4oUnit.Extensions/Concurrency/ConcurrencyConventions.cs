/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;

namespace Db4oUnit.Extensions.Concurrency
{
	public class ConcurrencyConventions
	{
		internal static string CheckMethodNameFor(string testMethodName)
		{
			int testPrefixLength = TestPrefix().Length;
			string subMethodName = Sharpen.Runtime.Substring(testMethodName, testPrefixLength
				);
			return CheckPrefix() + subMethodName;
		}

		private static string CheckPrefix()
		{
			if (Db4oUnitPlatform.IsPascalCase())
			{
				return "Check";
			}
			return "check";
		}

		public static string TestPrefix()
		{
			if (Db4oUnitPlatform.IsPascalCase())
			{
				return "Conc";
			}
			return "conc";
		}
	}
}
