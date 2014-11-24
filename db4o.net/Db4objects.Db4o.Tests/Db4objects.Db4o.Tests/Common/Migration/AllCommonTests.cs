/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Migration;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class AllCommonTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new AllCommonTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(Db4oMigrationTestSuite), typeof(FieldsToTypeHandlerMigrationTestCase
				), typeof(MigrationHopsTestCase), typeof(TranslatorToTypehandlerMigrationTestCase
				) };
		}
	}
}
