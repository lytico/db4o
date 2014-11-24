/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.Internal
{
	public class ObjectContainerFactory
	{
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		public static IEmbeddedObjectContainer OpenObjectContainer(IEmbeddedConfiguration
			 config, string databaseFileName)
		{
			IConfiguration legacyConfig = Db4oLegacyConfigurationBridge.AsLegacy(config);
			Config4Impl.AssertIsNotTainted(legacyConfig);
			EmitDebugInfo();
			IEmbeddedObjectContainer oc = new IoAdaptedObjectContainer(legacyConfig, databaseFileName
				);
			((EmbeddedConfigurationImpl)config).ApplyConfigurationItems(oc);
			Db4objects.Db4o.Internal.Messages.LogMsg(legacyConfig, 5, databaseFileName);
			return oc;
		}

		private static void EmitDebugInfo()
		{
		}
	}
}
