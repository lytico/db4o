/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Backup;

namespace Db4objects.Db4o.Tests.Common.Backup
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Backup.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(BackupFromMemoryBinIsAccessibleThroughStorageTestCase
				), typeof(BackupMemoryToFileTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(BackupStressTestCase) };
		}
		#endif // !SILVERLIGHT
	}
}
