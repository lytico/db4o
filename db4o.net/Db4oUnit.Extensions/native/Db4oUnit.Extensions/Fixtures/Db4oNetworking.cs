/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */
#if SILVERLIGHT

using System;
using System.Collections;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4oUnit.Fixtures;

namespace Db4oUnit.Extensions.Fixtures
{
	public class Db4oNetworking : IMultiSessionFixture
	{
		public Db4oNetworking()
		{
			throw new NotImplementedException();
		}

		public Db4oNetworking(string label)
		{
			throw new NotImplementedException();
		}

		string ILabeled.Label()
		{
			throw new NotImplementedException();
		}

		void IDb4oFixture.Open(IDb4oTestCase testInstance)
		{
			throw new NotImplementedException();
		}

		void IDb4oFixture.Close()
		{
			throw new NotImplementedException();
		}

		void IDb4oFixture.Reopen(IDb4oTestCase testInstance)
		{
			throw new NotImplementedException();
		}

		void IDb4oFixture.Clean()
		{
			throw new NotImplementedException();
		}

		LocalObjectContainer IDb4oFixture.FileSession()
		{
			throw new NotImplementedException();
		}

		IExtObjectContainer IDb4oFixture.Db()
		{
			throw new NotImplementedException();
		}

		IConfiguration IDb4oFixture.Config()
		{
			throw new NotImplementedException();
		}

		bool IDb4oFixture.Accept(Type clazz)
		{
			throw new NotImplementedException();
		}

		void IDb4oFixture.Defragment()
		{
			throw new NotImplementedException();
		}

		void IDb4oFixture.ConfigureAtRuntime(IRuntimeConfigureAction action)
		{
			throw new NotImplementedException();
		}

		void IDb4oFixture.FixtureConfiguration(IFixtureConfiguration configuration)
		{
			throw new NotImplementedException();
		}

		void IDb4oFixture.ResetConfig()
		{
			throw new NotImplementedException();
		}

		IList IDb4oFixture.UncaughtExceptions()
		{
			throw new NotImplementedException();
		}

		IExtObjectContainer IMultiSessionFixture.OpenNewSession(IDb4oTestCase testInstance)
		{
			throw new NotImplementedException();
		}
	}
}

#endif