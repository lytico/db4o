/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Freespace;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	public class FreespaceManagerTypeChangeSlotCountTestCase : TestWithTempFile
	{
		private const int Size = 10000;

		private LocalObjectContainer _container;

		private IClosure4 _currentConfig;

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(FreespaceManagerTypeChangeSlotCountTestCase)).Run();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestMigrateFromRamToBTree()
		{
			CreateDatabaseUsingRamManager();
			MigrateToBTree();
			Reopen();
			CreateFreeSpace();
			IList initialSlots = GetSlots(_container.FreespaceManager());
			Reopen();
			IList currentSlots = GetSlots(_container.FreespaceManager());
			Assert.AreEqual(initialSlots, currentSlots);
			_container.Close();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestMigrateFromBTreeToRam()
		{
			CreateDatabaseUsingBTreeManager();
			MigrateToRam();
			CreateFreeSpace();
			IList initialSlots = GetSlots(_container.FreespaceManager());
			Reopen();
			Assert.AreEqual(initialSlots, GetSlots(_container.FreespaceManager()));
			_container.Close();
		}

		private void Reopen()
		{
			_container.Close();
			Open();
		}

		private void CreateDatabaseUsingRamManager()
		{
			ConfigureRamFreespaceManager();
			Open();
		}

		private void CreateDatabaseUsingBTreeManager()
		{
			ConfigureBTreeFreespaceManager();
			Open();
		}

		private void Open()
		{
			IConfiguration config = ((IConfiguration)_currentConfig.Run());
			Db4oLegacyConfigurationBridge.AsIdSystemConfiguration(config).UsePointerBasedSystem
				();
			_container = (LocalObjectContainer)Db4oFactory.OpenFile(config, TempFile());
		}

		private void CreateFreeSpace()
		{
			Slot slot = _container.AllocateSlot(Size);
			_container.Free(slot);
		}

		/// <exception cref="System.Exception"></exception>
		private void MigrateToBTree()
		{
			_container.Close();
			ConfigureBTreeFreespaceManager();
			Open();
		}

		private void ConfigureBTreeFreespaceManager()
		{
			_currentConfig = new _IClosure4_86();
		}

		private sealed class _IClosure4_86 : IClosure4
		{
			public _IClosure4_86()
			{
			}

			public object Run()
			{
				IConfiguration config = Db4oFactory.NewConfiguration();
				config.Freespace().UseBTreeSystem();
				return config;
			}
		}

		/// <exception cref="System.Exception"></exception>
		private void MigrateToRam()
		{
			_container.Close();
			ConfigureRamFreespaceManager();
			Open();
		}

		private void ConfigureRamFreespaceManager()
		{
			_currentConfig = new _IClosure4_101();
		}

		private sealed class _IClosure4_101 : IClosure4
		{
			public _IClosure4_101()
			{
			}

			public object Run()
			{
				IConfiguration config = Db4oFactory.NewConfiguration();
				config.Freespace().UseRamSystem();
				return config;
			}
		}

		private IList GetSlots(IFreespaceManager freespaceManager)
		{
			IList retVal = new ArrayList();
			freespaceManager.Traverse(new _IVisitor4_110(retVal));
			return retVal;
		}

		private sealed class _IVisitor4_110 : IVisitor4
		{
			public _IVisitor4_110(IList retVal)
			{
				this.retVal = retVal;
			}

			public void Visit(object obj)
			{
				retVal.Add(obj);
			}

			private readonly IList retVal;
		}
	}
}
