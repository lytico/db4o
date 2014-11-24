/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;

namespace Db4oUnit.Extensions.Fixtures
{
	public interface ICustomClientServerConfiguration : IDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		void ConfigureServer(IConfiguration config);

		/// <exception cref="System.Exception"></exception>
		void ConfigureClient(IConfiguration config);
	}
}
