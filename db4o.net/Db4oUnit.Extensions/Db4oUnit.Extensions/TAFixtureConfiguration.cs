/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;

namespace Db4oUnit.Extensions
{
	/// <summary>
	/// Configure the test case to run with TransparentActivationSupport
	/// enabled unless the test case implements OptOutTA.
	/// </summary>
	/// <remarks>
	/// Configure the test case to run with TransparentActivationSupport
	/// enabled unless the test case implements OptOutTA.
	/// </remarks>
	public class TAFixtureConfiguration : IFixtureConfiguration
	{
		public virtual void Configure(IDb4oTestCase testCase, IConfiguration config)
		{
			if (testCase is IOptOutTA)
			{
				return;
			}
			config.Add(new TransparentActivationSupport());
		}

		public virtual string GetLabel()
		{
			return "TA";
		}
	}
}
