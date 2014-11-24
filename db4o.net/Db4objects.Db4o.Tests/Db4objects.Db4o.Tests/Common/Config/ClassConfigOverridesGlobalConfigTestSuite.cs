/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class ClassConfigOverridesGlobalConfigTestSuite : FixtureTestSuiteDescription
	{
		private static readonly FixtureVariable GlobalConfig = FixtureVariable.NewInstance
			("global");

		private static readonly FixtureVariable ClassConfig = FixtureVariable.NewInstance
			("class");

		public class ClassConfigOverridesGlobalConfigTestUnit : AbstractDb4oTestCase
		{
			public class Item
			{
			}

			protected override void Configure(IConfiguration config)
			{
				config.GenerateUUIDs(((ConfigScope)GlobalConfig.Value));
				if (!((TernaryBool)ClassConfig.Value).IsUnspecified())
				{
					config.ObjectClass(typeof(ClassConfigOverridesGlobalConfigTestSuite.ClassConfigOverridesGlobalConfigTestUnit.Item
						)).GenerateUUIDs(((TernaryBool)ClassConfig.Value).BooleanValue(true));
				}
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Store()
			{
				Store(new ClassConfigOverridesGlobalConfigTestSuite.ClassConfigOverridesGlobalConfigTestUnit.Item
					());
			}

			public virtual void TestNoUUIDIsGenerated()
			{
				ClassConfigOverridesGlobalConfigTestSuite.ClassConfigOverridesGlobalConfigTestUnit.Item
					 item = ((ClassConfigOverridesGlobalConfigTestSuite.ClassConfigOverridesGlobalConfigTestUnit.Item
					)RetrieveOnlyInstance(typeof(ClassConfigOverridesGlobalConfigTestSuite.ClassConfigOverridesGlobalConfigTestUnit.Item
					)));
				IObjectInfo objectInfo = Db().Ext().GetObjectInfo(item);
				if (!((TernaryBool)ClassConfig.Value).IsUnspecified())
				{
					AssertGeneration(objectInfo, ((TernaryBool)ClassConfig.Value).BooleanValue(true) 
						&& ((ConfigScope)GlobalConfig.Value) != ConfigScope.Disabled);
				}
				else
				{
					AssertGeneration(objectInfo, ((ConfigScope)GlobalConfig.Value) == ConfigScope.Globally
						);
				}
			}

			private void AssertGeneration(IObjectInfo objectInfo, bool expectGeneration)
			{
				if (expectGeneration)
				{
					Assert.IsNotNull(objectInfo.GetUUID());
				}
				else
				{
					Assert.IsNull(objectInfo.GetUUID());
					Assert.AreEqual(0L, objectInfo.GetCommitTimestamp());
				}
			}
		}

		public ClassConfigOverridesGlobalConfigTestSuite()
		{
			{
				TestUnits(new Type[] { typeof(ClassConfigOverridesGlobalConfigTestSuite.ClassConfigOverridesGlobalConfigTestUnit
					) });
				FixtureProviders(new IFixtureProvider[] { new SimpleFixtureProvider(GlobalConfig, 
					new object[] { ConfigScope.Globally, ConfigScope.Individually, ConfigScope.Disabled
					 }), new SimpleFixtureProvider(ClassConfig, new object[] { TernaryBool.Yes, TernaryBool
					.No, TernaryBool.Unspecified }) });
			}
		}
	}
}
