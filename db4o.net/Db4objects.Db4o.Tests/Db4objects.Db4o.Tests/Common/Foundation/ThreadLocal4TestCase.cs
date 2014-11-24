/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class ThreadLocal4TestCase : ITestCase
	{
		public virtual void TestSet()
		{
			object value = new object();
			ThreadLocal4 local = new ThreadLocal4();
			local.Set(value);
			Assert.AreSame(value, local.Get());
			local.Set(null);
			Assert.AreSame(null, local.Get());
		}
	}
}
