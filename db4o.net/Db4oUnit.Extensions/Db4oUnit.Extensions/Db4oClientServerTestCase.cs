/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4oUnit.Extensions
{
	public class Db4oClientServerTestCase : AbstractDb4oTestCase, IOptOutSolo
	{
		public virtual IDb4oClientServerFixture ClientServerFixture()
		{
			return (IDb4oClientServerFixture)Fixture();
		}
	}
}
