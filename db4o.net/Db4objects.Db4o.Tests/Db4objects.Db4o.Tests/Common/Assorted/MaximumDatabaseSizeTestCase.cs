/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class MaximumDatabaseSizeTestCase : AbstractDb4oTestCase, IOptOutNetworkingCS
		, IOptOutDefragSolo
	{
		public static void Main(string[] args)
		{
			new MaximumDatabaseSizeTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			ConfigureMaximumFileSize(config);
		}

		private void ConfigureMaximumFileSize(IConfiguration config)
		{
			Db4oLegacyConfigurationBridge.AsFileConfiguration(config).MaximumDatabaseFileSize
				 = 10000;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Store(new MaximumDatabaseSizeTestCase.SmallItem());
			bool exceptionOccurred = false;
			bool duringCommit = false;
			try
			{
				for (int i = 0; i < 100000; i++)
				{
					duringCommit = false;
					Store(new MaximumDatabaseSizeTestCase.BigItem());
					duringCommit = true;
					Commit();
				}
			}
			catch (DatabaseMaximumSizeReachedException)
			{
				exceptionOccurred = true;
			}
			Assert.IsTrue(exceptionOccurred);
			Fixture().ConfigureAtRuntime(new _IRuntimeConfigureAction_42(this));
			Reopen();
			MaximumDatabaseSizeTestCase.SmallItem smallItem = ((MaximumDatabaseSizeTestCase.SmallItem
				)RetrieveOnlyInstance(typeof(MaximumDatabaseSizeTestCase.SmallItem)));
			Assert.IsNotNull(smallItem);
			Defragment();
			smallItem = ((MaximumDatabaseSizeTestCase.SmallItem)RetrieveOnlyInstance(typeof(MaximumDatabaseSizeTestCase.SmallItem
				)));
			Assert.IsNotNull(smallItem);
		}

		private sealed class _IRuntimeConfigureAction_42 : IRuntimeConfigureAction
		{
			public _IRuntimeConfigureAction_42(MaximumDatabaseSizeTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(IConfiguration config)
			{
				this._enclosing.ConfigureMaximumFileSize(config);
				config.ReadOnly(true);
			}

			private readonly MaximumDatabaseSizeTestCase _enclosing;
		}

		public class SmallItem
		{
		}

		public class BigItem
		{
			public byte[] bytes = new byte[2];
		}
	}
}
