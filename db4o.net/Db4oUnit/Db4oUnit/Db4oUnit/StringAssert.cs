/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;

namespace Db4oUnit
{
	public class StringAssert
	{
		public static void Contains(string expected, string actual)
		{
			if (actual.IndexOf(expected) >= 0)
			{
				return;
			}
			Assert.Fail("'" + actual + "' does not contain '" + expected + "'");
		}
	}
}
