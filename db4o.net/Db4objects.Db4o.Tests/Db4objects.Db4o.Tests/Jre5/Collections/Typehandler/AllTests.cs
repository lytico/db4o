/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] arguments)
		{
			new Db4objects.Db4o.Tests.Jre5.Collections.Typehandler.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ListTypeHandlerCascadedDeleteTestCase), typeof(ListTypeHandlerPersistedCountTestCase
				), typeof(ListTypeHandlerTestSuite), typeof(ListTypeHandlerGreaterSmallerTestSuite
				), typeof(ListTypeHandlerStringElementTestSuite), typeof(MapTypeHandlerTestSuite
				), typeof(NamedArrayListTypeHandlerTestCase), typeof(SimpleListTestCase), typeof(
				SimpleListQueryTestCase) };
		}
	}
}
