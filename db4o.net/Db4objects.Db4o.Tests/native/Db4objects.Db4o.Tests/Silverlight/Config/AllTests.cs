/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
#if SILVERLIGHT

using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Silverlight.Config
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new[] { typeof(SilverlightSupportTestCase), };
		}
	}
}

#endif