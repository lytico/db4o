/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class CsDisposableTestCase : AbstractDb4oTestCase
	{
		public void TestDispose()
		{
			Assert.IsTrue(!Db().IsClosed());
			Db().Dispose();
			Assert.IsTrue(Db().IsClosed());
		}
	}
}
