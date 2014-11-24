/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;

namespace Db4oUnit.Extensions
{
	public interface IFixtureConfiguration
	{
		string GetLabel();

		void Configure(IDb4oTestCase testCase, IConfiguration config);
	}
}
