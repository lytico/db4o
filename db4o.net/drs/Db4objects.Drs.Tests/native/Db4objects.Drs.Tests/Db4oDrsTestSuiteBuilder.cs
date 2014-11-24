/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
namespace Db4objects.Drs.Tests
{
	partial class Db4oDrsTestSuiteBuilder
	{
		private static void Exit(int errorCount)
		{
#if !CF
			System.Environment.Exit(errorCount);
#endif
		}
	}
}
