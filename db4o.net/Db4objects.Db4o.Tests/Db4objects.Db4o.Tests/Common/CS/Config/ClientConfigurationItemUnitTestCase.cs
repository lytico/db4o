/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions.Dbmock;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.CS.Config;
using Sharpen.Util;

namespace Db4objects.Db4o.Tests.Common.CS.Config
{
	public class ClientConfigurationItemUnitTestCase : ITestLifeCycle
	{
		private IList _applied;

		private ClientConfigurationImpl _config;

		public virtual void TestPrepareApply()
		{
			IList items = Arrays.AsList(new ClientConfigurationItemUnitTestCase.DummyConfigurationItem
				[] { new ClientConfigurationItemUnitTestCase.DummyConfigurationItem(_applied), new 
				ClientConfigurationItemUnitTestCase.DummyConfigurationItem(_applied) });
			for (IEnumerator itemIter = items.GetEnumerator(); itemIter.MoveNext(); )
			{
				ClientConfigurationItemUnitTestCase.DummyConfigurationItem item = ((ClientConfigurationItemUnitTestCase.DummyConfigurationItem
					)itemIter.Current);
				_config.AddConfigurationItem(item);
				Assert.AreEqual(1, item.PrepareCount());
			}
			Assert.AreEqual(0, _applied.Count);
			_config.ApplyConfigurationItems(new MockClient());
			AssertListsAreEqual(items, _applied);
			for (IEnumerator itemIter = items.GetEnumerator(); itemIter.MoveNext(); )
			{
				ClientConfigurationItemUnitTestCase.DummyConfigurationItem item = ((ClientConfigurationItemUnitTestCase.DummyConfigurationItem
					)itemIter.Current);
				Assert.AreEqual(1, item.PrepareCount());
			}
		}

		public virtual void TestAddTwice()
		{
			ClientConfigurationItemUnitTestCase.DummyConfigurationItem item = new ClientConfigurationItemUnitTestCase.DummyConfigurationItem
				(_applied);
			_config.AddConfigurationItem(item);
			_config.AddConfigurationItem(item);
			_config.ApplyConfigurationItems(new MockClient());
			Assert.AreEqual(1, item.PrepareCount());
			AssertListsAreEqual(Arrays.AsList(new ClientConfigurationItemUnitTestCase.DummyConfigurationItem
				[] { item }), _applied);
		}

		private void AssertListsAreEqual(IList a, IList b)
		{
			Assert.AreEqual(a.Count, b.Count);
			for (int i = 0; i < a.Count; i++)
			{
				Assert.AreEqual(a[i], b[i]);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_applied = new ArrayList();
			_config = (ClientConfigurationImpl)Db4oClientServer.NewClientConfiguration();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}

		private class DummyConfigurationItem : IClientConfigurationItem
		{
			private int _prepareCount = 0;

			private IList _applied;

			public DummyConfigurationItem(IList applied)
			{
				_applied = applied;
			}

			public virtual void Apply(IExtClient client)
			{
				_applied.Add(this);
			}

			public virtual void Prepare(IClientConfiguration configuration)
			{
				_prepareCount++;
			}

			public virtual int PrepareCount()
			{
				return _prepareCount;
			}
		}
	}
}
#endif // !SILVERLIGHT
