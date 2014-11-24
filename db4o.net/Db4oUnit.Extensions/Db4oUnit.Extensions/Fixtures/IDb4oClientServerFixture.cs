/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;

namespace Db4oUnit.Extensions.Fixtures
{
	public interface IDb4oClientServerFixture : IDb4oFixture, IMultiSessionFixture
	{
		IObjectServer Server();

		int ServerPort();
	}
}
