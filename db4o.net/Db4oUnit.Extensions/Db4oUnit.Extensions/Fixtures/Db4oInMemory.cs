/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;

namespace Db4oUnit.Extensions.Fixtures
{
	public class Db4oInMemory : AbstractSoloDb4oFixture
	{
		private static readonly string DbUri = "test_db";

		public Db4oInMemory() : base()
		{
		}

		public Db4oInMemory(IFixtureConfiguration fc) : this()
		{
			FixtureConfiguration(fc);
		}

		public override bool Accept(Type clazz)
		{
			if (!base.Accept(clazz))
			{
				return false;
			}
			if (typeof(IOptOutInMemory).IsAssignableFrom(clazz))
			{
				return false;
			}
			return true;
		}

		private readonly PagingMemoryStorage _storage = new PagingMemoryStorage(63);

		protected override IObjectContainer CreateDatabase(IConfiguration config)
		{
			return Db4oFactory.OpenFile(config, DbUri);
		}

		protected override IConfiguration NewConfiguration()
		{
			IConfiguration config = base.NewConfiguration();
			config.Storage = _storage;
			return config;
		}

		protected override void DoClean()
		{
			try
			{
				_storage.Delete(DbUri);
			}
			catch (IOException exc)
			{
				Sharpen.Runtime.PrintStackTrace(exc);
			}
		}

		public override string Label()
		{
			return BuildLabel("IN-MEMORY");
		}

		/// <exception cref="System.Exception"></exception>
		public override void Defragment()
		{
			Defragment(DbUri);
		}
	}
}
