/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Activation;

namespace Db4objects.Db4o.Tests.Common.Activation
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Activation.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ActivationDepthProviderConfigTestCase), typeof(MaximumActivationDepthTestCase
				), typeof(TransparentActivationDepthTestCase) };
		}
	}
}
