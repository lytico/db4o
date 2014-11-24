/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
#if SILVERLIGHT

using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Silverlight.IO
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			       	{
			       		typeof(IsolatedStorageFileBinTestSuite),
						typeof(IsolatedStorageStorageTestCase),
			       	};
		}
	}
}

#endif