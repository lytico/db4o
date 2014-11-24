/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Types;

namespace Db4objects.Db4o.Tests.Common.Types
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Types.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(Db4objects.Db4o.Tests.Common.Types.Arrays.AllTests
				), typeof(StoreTopLevelPrimitiveTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(StoreExceptionTestCase) };
		}
		#endif // !SILVERLIGHT
		// Storing exceptions is not supported on Silverlight. 
	}
}
