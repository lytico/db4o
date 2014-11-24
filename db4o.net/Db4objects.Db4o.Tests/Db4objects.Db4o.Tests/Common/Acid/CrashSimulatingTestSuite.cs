/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Tests.Common.Acid;

namespace Db4objects.Db4o.Tests.Common.Acid
{
	/// <exclude></exclude>
	public class CrashSimulatingTestSuite : FixtureBasedTestSuite, IOptOutVerySlow
	{
		internal const bool Verbose = false;

		private static readonly FixtureVariable UseCache = new FixtureVariable();

		private static readonly FixtureVariable UseLogfile = new FixtureVariable();

		private static readonly FixtureVariable WriteTrash = new FixtureVariable();

		private static readonly FixtureVariable IdSystem = new FixtureVariable();

		private static readonly FixtureVariable FreespaceManager = new FixtureVariable();

		private IFixtureProvider[] SingleConfig()
		{
			return new IFixtureProvider[] { new SimpleFixtureProvider(UseCache, new CrashSimulatingTestSuite.LabeledBoolean
				[] { new CrashSimulatingTestSuite.LabeledBoolean("no cache", false) }), new SimpleFixtureProvider
				(UseLogfile, new CrashSimulatingTestSuite.LabeledBoolean[] { new CrashSimulatingTestSuite.LabeledBoolean
				("no logfile", false) }), new SimpleFixtureProvider(WriteTrash, new CrashSimulatingTestSuite.LabeledBoolean
				[] { new CrashSimulatingTestSuite.LabeledBoolean("write trash", true) }), new SimpleFixtureProvider
				(FreespaceManager, new CrashSimulatingTestSuite.LabeledConfig[] { new _LabeledConfig_44
				("BTreeFreespaceManager") }), new SimpleFixtureProvider(IdSystem, new CrashSimulatingTestSuite.LabeledConfig
				[] { new _LabeledConfig_52("BTreeIdSystem") }) };
		}

		private sealed class _LabeledConfig_44 : CrashSimulatingTestSuite.LabeledConfig
		{
			public _LabeledConfig_44(string baseArg1) : base(baseArg1)
			{
			}

			public override void Configure(Config4Impl config)
			{
				// config.freespace().useRamSystem();
				config.Freespace().UseBTreeSystem();
			}
		}

		private sealed class _LabeledConfig_52 : CrashSimulatingTestSuite.LabeledConfig
		{
			public _LabeledConfig_52(string baseArg1) : base(baseArg1)
			{
			}

			public override void Configure(Config4Impl config)
			{
				Db4oLegacyConfigurationBridge.AsIdSystemConfiguration(config).UseStackedBTreeSystem
					();
			}
		}

		// Db4oLegacyConfigurationBridge.asIdSystemConfiguration(config).useInMemorySystem();
		// Db4oLegacyConfigurationBridge.asIdSystemConfiguration(config).usePointerBasedSystem();
		public override IFixtureProvider[] FixtureProviders()
		{
			//		if(true){
			//			return singleConfig();
			//		}
			return new IFixtureProvider[] { new SimpleFixtureProvider(UseCache, new CrashSimulatingTestSuite.LabeledBoolean
				[] { new CrashSimulatingTestSuite.LabeledBoolean("cached", true), new CrashSimulatingTestSuite.LabeledBoolean
				("no cache", false) }), new SimpleFixtureProvider(UseLogfile, new CrashSimulatingTestSuite.LabeledBoolean
				[] { new CrashSimulatingTestSuite.LabeledBoolean("logfile", true), new CrashSimulatingTestSuite.LabeledBoolean
				("no logfile", false) }), new SimpleFixtureProvider(WriteTrash, new CrashSimulatingTestSuite.LabeledBoolean
				[] { new CrashSimulatingTestSuite.LabeledBoolean("write trash", true), new CrashSimulatingTestSuite.LabeledBoolean
				("don't write trash", false) }), new SimpleFixtureProvider(FreespaceManager, new 
				CrashSimulatingTestSuite.LabeledConfig[] { new _LabeledConfig_76("InMemoryFreespaceManager"
				), new _LabeledConfig_80("BTreeFreespaceManager") }), new SimpleFixtureProvider(
				IdSystem, new CrashSimulatingTestSuite.LabeledConfig[] { new _LabeledConfig_88("PointerBasedIdSystem"
				), new _LabeledConfig_92("BTreeIdSystem"), new _LabeledConfig_96("InMemoryIdSystem"
				) }) };
		}

		private sealed class _LabeledConfig_76 : CrashSimulatingTestSuite.LabeledConfig
		{
			public _LabeledConfig_76(string baseArg1) : base(baseArg1)
			{
			}

			public override void Configure(Config4Impl config)
			{
				config.Freespace().UseRamSystem();
			}
		}

		private sealed class _LabeledConfig_80 : CrashSimulatingTestSuite.LabeledConfig
		{
			public _LabeledConfig_80(string baseArg1) : base(baseArg1)
			{
			}

			public override void Configure(Config4Impl config)
			{
				config.Freespace().UseBTreeSystem();
			}
		}

		private sealed class _LabeledConfig_88 : CrashSimulatingTestSuite.LabeledConfig
		{
			public _LabeledConfig_88(string baseArg1) : base(baseArg1)
			{
			}

			public override void Configure(Config4Impl config)
			{
				Db4oLegacyConfigurationBridge.AsIdSystemConfiguration(config).UsePointerBasedSystem
					();
			}
		}

		private sealed class _LabeledConfig_92 : CrashSimulatingTestSuite.LabeledConfig
		{
			public _LabeledConfig_92(string baseArg1) : base(baseArg1)
			{
			}

			public override void Configure(Config4Impl config)
			{
				Db4oLegacyConfigurationBridge.AsIdSystemConfiguration(config).UseStackedBTreeSystem
					();
			}
		}

		private sealed class _LabeledConfig_96 : CrashSimulatingTestSuite.LabeledConfig
		{
			public _LabeledConfig_96(string baseArg1) : base(baseArg1)
			{
			}

			public override void Configure(Config4Impl config)
			{
				Db4oLegacyConfigurationBridge.AsIdSystemConfiguration(config).UseInMemorySystem();
			}
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(CrashSimulatingTestSuite.CrashSimulatingTestCase) };
		}

		public class CrashSimulatingTestCase : ITestCase, IOptOutMultiSession, IOptOutVerySlow
		{
			// The cache may touch more bytes than the ones we modified.
			// We should be safe even if we don't get this test to pass.
			// The log file is not a public API yet anyway.
			// It's only needed for the PointerBasedIdSystem
			// With the new BTreeIdSystem it's not likely to become important
			// so we can safely ignore the failing write trash case.
			private IConfiguration BaseConfig(bool useLogFile)
			{
				Config4Impl config = (Config4Impl)Db4oFactory.NewConfiguration();
				config.ObjectClass(typeof(CrashSimulatingTestSuite.CrashData)).ObjectField("_name"
					).Indexed(true);
				config.ReflectWith(Platform4.ReflectorForType(typeof(CrashSimulatingTestSuite.CrashSimulatingTestCase
					)));
				config.BTreeNodeSize(4);
				config.LockDatabaseFile(false);
				config.FileBasedTransactionLog(useLogFile);
				((CrashSimulatingTestSuite.LabeledConfig)IdSystem.Value).Configure(config);
				((CrashSimulatingTestSuite.LabeledConfig)FreespaceManager.Value).Configure(config
					);
				return config;
			}

			private void CheckFiles(bool useLogFile, string fileName, string infix, int count
				)
			{
				for (int i = 1; i <= count; i++)
				{
					string versionedFileName = fileName + infix + i;
					IObjectContainer oc = Db4oFactory.OpenFile(BaseConfig(useLogFile), versionedFileName
						);
					try
					{
						if (!StateBeforeCommit(oc))
						{
							if (!StateAfterFirstCommit(oc))
							{
								Assert.IsTrue(StateAfterSecondCommit(oc));
							}
						}
					}
					finally
					{
						oc.Close();
					}
				}
			}

			private bool StateBeforeCommit(IObjectContainer oc)
			{
				return Expect(oc, new string[] { "one", "two", "three" });
			}

			private bool StateAfterFirstCommit(IObjectContainer oc)
			{
				return Expect(oc, new string[] { "one", "two", "four", "five", "six", "seven", "eight"
					, "nine", "10", "11", "12", "13", "14" });
			}

			private bool StateAfterSecondCommit(IObjectContainer oc)
			{
				return Expect(oc, new string[] { "10", "13" });
			}

			private bool Expect(IObjectContainer container, string[] names)
			{
				Collection4 expected = new Collection4(names);
				IObjectSet actual = container.Query(typeof(CrashSimulatingTestSuite.CrashData));
				while (actual.HasNext())
				{
					CrashSimulatingTestSuite.CrashData current = (CrashSimulatingTestSuite.CrashData)
						actual.Next();
					if (!expected.Remove(current._name))
					{
						return false;
					}
				}
				return expected.IsEmpty();
			}

			/// <exception cref="System.IO.IOException"></exception>
			private void CreateFile(IConfiguration config, string fileName)
			{
				IObjectContainer oc = Db4oFactory.OpenFile(config, fileName);
				try
				{
					Populate(oc);
				}
				finally
				{
					oc.Close();
				}
				File4.Copy(fileName, fileName + "0");
			}

			private void Populate(IObjectContainer container)
			{
				for (int i = 0; i < 10; i++)
				{
					container.Store(new CrashSimulatingTestSuite.Item("delme"));
				}
				CrashSimulatingTestSuite.CrashData one = new CrashSimulatingTestSuite.CrashData(null
					, "one");
				CrashSimulatingTestSuite.CrashData two = new CrashSimulatingTestSuite.CrashData(one
					, "two");
				CrashSimulatingTestSuite.CrashData three = new CrashSimulatingTestSuite.CrashData
					(one, "three");
				container.Store(one);
				container.Store(two);
				container.Store(three);
				container.Commit();
				IObjectSet objectSet = container.Query(typeof(CrashSimulatingTestSuite.Item));
				while (objectSet.HasNext())
				{
					container.Delete(objectSet.Next());
				}
			}
		}

		public class CrashData
		{
			public string _name;

			public CrashSimulatingTestSuite.CrashData _next;

			public CrashData(CrashSimulatingTestSuite.CrashData next_, string name)
			{
				_next = next_;
				_name = name;
			}

			public override string ToString()
			{
				return _name + " -> " + _next;
			}
		}

		public class Item
		{
			public string name;

			public Item()
			{
			}

			public Item(string name_)
			{
				this.name = name_;
			}

			public virtual string GetName()
			{
				return name;
			}

			public virtual void SetName(string name_)
			{
				name = name_;
			}
		}

		public class LabeledBoolean : ILabeled
		{
			private readonly bool _value;

			private readonly string _label;

			public LabeledBoolean(string label, bool value)
			{
				_label = label;
				_value = value;
			}

			public virtual string Label()
			{
				return _label;
			}

			public virtual bool BooleanValue()
			{
				return _value;
			}
		}

		public abstract class LabeledConfig : ILabeled
		{
			private readonly string _label;

			public LabeledConfig(string label)
			{
				_label = label;
			}

			public abstract void Configure(Config4Impl config);

			public virtual string Label()
			{
				return _label;
			}
		}
	}
}
