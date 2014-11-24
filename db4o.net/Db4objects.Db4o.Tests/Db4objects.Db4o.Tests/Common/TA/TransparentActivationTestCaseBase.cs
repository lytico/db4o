/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class TransparentActivationTestCaseBase : AbstractDb4oTestCase, IOptOutTA
	{
		public TransparentActivationTestCaseBase() : base()
		{
		}

		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
			config.GenerateUUIDs(ConfigScope.Globally);
		}
	}
}
