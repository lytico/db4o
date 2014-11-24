/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class TransientConfigurationTestSuite : FixtureTestSuiteDescription
	{
		private static readonly FixtureVariable ClassConfig = FixtureVariable.NewInstance
			("Class");

		public class TransientConfigurationTestUnit : AbstractDb4oTestCase
		{
			private const int TransientValue = 42;

			private const int PersistentValue = unchecked((int)(0xdb40));

			/// <exception cref="System.Exception"></exception>
			protected override void Configure(IConfiguration config)
			{
				config.ObjectClass(typeof(TransientConfigurationTestSuite.Item)).StoreTransientFields
					((((bool)ClassConfig.Value)));
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Store()
			{
				Store(new TransientConfigurationTestSuite.Item(TransientValue, PersistentValue));
			}

			public virtual void TestRetrieval()
			{
				TransientConfigurationTestSuite.Item instance = ((TransientConfigurationTestSuite.Item
					)RetrieveOnlyInstance(typeof(TransientConfigurationTestSuite.Item)));
				Assert.AreEqual(PersistentValue, instance._persistentField);
				Assert.AreEqual(ExpectedTransientValue(), instance._transientField);
			}

			private int ExpectedTransientValue()
			{
				return (((bool)ClassConfig.Value)) ? TransientValue : 0;
			}
		}

		public class Item
		{
			[System.NonSerialized]
			public int _transientField;

			public int _persistentField;

			public Item(int transientValue, int persistentValue)
			{
				_transientField = transientValue;
				_persistentField = persistentValue;
			}
		}

		public TransientConfigurationTestSuite()
		{
			{
				TestUnits(new Type[] { typeof(TransientConfigurationTestSuite.TransientConfigurationTestUnit
					) });
				FixtureProviders(new IFixtureProvider[] { new SimpleFixtureProvider(ClassConfig, 
					new object[] { true, false }) });
			}
		}
	}
}
