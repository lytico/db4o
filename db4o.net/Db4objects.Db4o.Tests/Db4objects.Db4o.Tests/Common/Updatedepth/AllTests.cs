/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Updatedepth;

namespace Db4objects.Db4o.Tests.Common.Updatedepth
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(NegativeUpdateDepthTestCase), typeof(UpdateDepthWithCascadingDeleteTestCase
				) };
		}
	}
}
