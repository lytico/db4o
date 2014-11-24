/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Freespace;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Common.Migration;
using Db4objects.Db4o.Tests.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class Db4oMigrationTestSuite : ITestSuiteBuilder
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(Db4oMigrationTestSuite)).Run();
		}

		public virtual IEnumerator GetEnumerator()
		{
			return new Db4oMigrationSuiteBuilder(TestCases(), Libraries()).GetEnumerator();
		}

		protected virtual string[] Libraries()
		{
			if (true)
			{
				return Db4oMigrationSuiteBuilder.All;
			}
			if (true)
			{
				// run against specific libraries + the current one
				string javaPath = "db4o.archives/java1.2/db4o-5.7-java1.2.jar";
				string netPath = "db4o.archives/net-2.0/7.4/Db4objects.Db4o.dll";
				return new string[] { WorkspaceServices.WorkspacePath(javaPath) };
			}
			return Db4oMigrationSuiteBuilder.Current;
		}

		protected virtual Type[] TestCases()
		{
			Type[] classes = new Type[] { typeof(BooleanHandlerUpdateTestCase), typeof(ByteHandlerUpdateTestCase
				), typeof(CascadedDeleteFileFormatUpdateTestCase), typeof(CharHandlerUpdateTestCase
				), typeof(DateHandlerUpdateTestCase), typeof(DeletionUponFormatMigrationTestCase
				), typeof(DoubleHandlerUpdateTestCase), typeof(FloatHandlerUpdateTestCase), typeof(
				IntHandlerUpdateTestCase), typeof(InterfaceHandlerUpdateTestCase), typeof(LongHandlerUpdateTestCase
				), typeof(MultiDimensionalArrayHandlerUpdateTestCase), typeof(NestedArrayUpdateTestCase
				), typeof(ObjectArrayUpdateTestCase), typeof(PlainObjectUpdateTestCase), typeof(
				QueryingMigrationTestCase), typeof(ShortHandlerUpdateTestCase), typeof(StringHandlerUpdateTestCase
				), typeof(IxFreespaceMigrationTestCase), typeof(FreespaceManagerMigrationTestCase
				), typeof(CommitTimestampMigrationTestCase) };
			// EncryptedFileMigrationTestCase.class,  fails the 8.0 build, turned off temporarily
			// Order to run freespace/Encrypted tests last is
			// deliberate. Global configuration Db4o.configure()
			// is changed in the #setUp call and reused.
			return AddJavaTestCases(classes);
		}

		protected virtual Type[] AddJavaTestCases(Type[] classes)
		{
			Type[] javaTestCases = null;
			if (javaTestCases == null)
			{
				return classes;
			}
			int len = javaTestCases.Length;
			Type[] allClasses = new Type[classes.Length + len];
			System.Array.Copy(javaTestCases, 0, allClasses, 0, len);
			System.Array.Copy(classes, 0, allClasses, len, classes.Length);
			return allClasses;
		}
	}
}
