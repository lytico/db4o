/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions.Dbmock;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Tests.Common.Config;
using Sharpen.Util;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class EmbeddedConfigurationItemUnitTestCase : ITestLifeCycle
	{
		private IList _applied;

		private EmbeddedConfigurationImpl _config;

		public virtual void TestPrepareApply()
		{
			IList items = Arrays.AsList(new EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem
				[] { new EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem(_applied), 
				new EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem(_applied) });
			for (IEnumerator itemIter = items.GetEnumerator(); itemIter.MoveNext(); )
			{
				EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem item = ((EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem
					)itemIter.Current);
				_config.AddConfigurationItem(item);
				Assert.AreEqual(1, item.PrepareCount());
			}
			Assert.AreEqual(0, _applied.Count);
			_config.ApplyConfigurationItems(new MockEmbedded());
			AssertListsAreEqual(items, _applied);
			for (IEnumerator itemIter = items.GetEnumerator(); itemIter.MoveNext(); )
			{
				EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem item = ((EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem
					)itemIter.Current);
				Assert.AreEqual(1, item.PrepareCount());
			}
		}

		public virtual void TestAddTwice()
		{
			EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem item = new EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem
				(_applied);
			_config.AddConfigurationItem(item);
			_config.AddConfigurationItem(item);
			_config.ApplyConfigurationItems(new MockEmbedded());
			Assert.AreEqual(1, item.PrepareCount());
			AssertListsAreEqual(Arrays.AsList(new EmbeddedConfigurationItemUnitTestCase.DummyConfigurationItem
				[] { item }), _applied);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_applied = new ArrayList();
			_config = (EmbeddedConfigurationImpl)Db4oEmbedded.NewConfiguration();
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
		public virtual void TearDown()
		{
		}

		private class DummyConfigurationItem : IEmbeddedConfigurationItem
		{
			private int _prepareCount = 0;

			private IList _applied;

			public DummyConfigurationItem(IList applied)
			{
				_applied = applied;
			}

			public virtual void Apply(IEmbeddedObjectContainer container)
			{
				_applied.Add(this);
			}

			public virtual void Prepare(IEmbeddedConfiguration configuration)
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
