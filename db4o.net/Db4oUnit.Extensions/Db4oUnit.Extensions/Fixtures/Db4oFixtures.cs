/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions.Fixtures;

namespace Db4oUnit.Extensions.Fixtures
{
	public class Db4oFixtures
	{
		public static Db4oSolo NewSolo()
		{
			return new Db4oSolo();
		}

		public static Db4oInMemory NewInMemory()
		{
			return new Db4oInMemory();
		}

		public static IMultiSessionFixture NewEmbedded()
		{
			return new Db4oEmbeddedSessionFixture();
		}

		public static IMultiSessionFixture NewEmbedded(string label)
		{
			return new Db4oEmbeddedSessionFixture(label);
		}

		public static Db4oNetworking NewNetworkingCS()
		{
			return new Db4oNetworking();
		}

		public static Db4oNetworking NewNetworkingCS(string label)
		{
			return new Db4oNetworking(label);
		}
	}
}
