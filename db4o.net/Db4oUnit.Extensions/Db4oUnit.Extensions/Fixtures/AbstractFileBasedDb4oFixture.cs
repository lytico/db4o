/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oUnit.Extensions.Fixtures
{
	public abstract class AbstractFileBasedDb4oFixture : AbstractSoloDb4oFixture
	{
		private readonly Sharpen.IO.File _databaseFile;

		public AbstractFileBasedDb4oFixture()
		{
			string fileName = FileName();
			_databaseFile = new Sharpen.IO.File(CrossPlatformServices.DatabasePath(fileName));
		}

		protected abstract string FileName();

		protected override IObjectContainer CreateDatabase(IConfiguration config)
		{
			return Db4oFactory.OpenFile(config, GetAbsolutePath());
		}

		public virtual string GetAbsolutePath()
		{
			return _databaseFile.GetAbsolutePath();
		}

		/// <exception cref="System.Exception"></exception>
		public override void Defragment()
		{
			Defragment(GetAbsolutePath());
		}

		protected override void DoClean()
		{
			if (_databaseFile.Exists())
			{
				_databaseFile.Delete();
			}
		}
	}
}
