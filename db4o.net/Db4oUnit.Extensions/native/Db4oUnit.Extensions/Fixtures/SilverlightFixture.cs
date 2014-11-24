/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */
#if SILVERLIGHT

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.IO;

namespace Db4oUnit.Extensions.Fixtures
{
	public class SilverlightFixture : AbstractSoloDb4oFixture
	{
		protected override void DoClean()
		{
			if (null != _storage)
			{
				_storage.Delete(DatabaseFileName);
			}
		}

		public override string Label()
		{
			return BuildLabel("Silverlight Solo");
		}

		public override void Defragment()
		{
			Defragment(DatabaseFileName);
		}

		protected override IObjectContainer CreateDatabase(IConfiguration config)
		{
			_storage = config.Storage;
			return Db4oFactory.OpenFile(config, DatabaseFileName);
		}

		protected override IConfiguration NewConfiguration()
		{
			var config = base.NewConfiguration();
			var embeddedConfig = new EmbeddedConfigurationImpl(config);
			embeddedConfig.AddConfigurationItem(new SilverlightSupport());

			return config;
		}

		public override bool Accept(System.Type clazz)
		{
			if (typeof(IOptOutSilverlight).IsAssignableFrom(clazz)) return false;
			return base.Accept(clazz);
		}

		private const string DatabaseFileName = "SilverlightDatabase.db4o";
		private IStorage _storage;
	}
}

#endif