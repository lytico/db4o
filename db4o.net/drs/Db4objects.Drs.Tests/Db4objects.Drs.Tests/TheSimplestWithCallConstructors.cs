/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class TheSimplestWithCallConstructors : TheSimplest
	{
		protected override void Configure(IConfiguration config)
		{
			config.CallConstructors(true);
		}

		protected override SPCChild CreateChildObject(string name)
		{
			return new SPCChildWithoutDefaultConstructor(name);
		}
	}
}
