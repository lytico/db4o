/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4oUnit.Extensions
{
	public interface IDb4oFixture : ILabeled
	{
		/// <exception cref="System.Exception"></exception>
		void Open(IDb4oTestCase testInstance);

		/// <exception cref="System.Exception"></exception>
		void Close();

		/// <exception cref="System.Exception"></exception>
		void Reopen(IDb4oTestCase testInstance);

		void Clean();

		LocalObjectContainer FileSession();

		IExtObjectContainer Db();

		IConfiguration Config();

		bool Accept(Type clazz);

		/// <exception cref="System.Exception"></exception>
		void Defragment();

		void ConfigureAtRuntime(IRuntimeConfigureAction action);

		void FixtureConfiguration(IFixtureConfiguration configuration);

		void ResetConfig();

		IList UncaughtExceptions();
	}
}
