/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
	public class GenericCollectionTypeHandlerTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override Type[] TestUnits()
		{
			return new Type[]
			       	{
			       		typeof(GenericCollectionTypeHandlerTestUnit),
			       	};
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[]
			       	{
			       		new Db4oFixtureProvider(),
						GenericCollectionTypeHandlerTestVariables.CollectionFixtureProvider,
						GenericCollectionTypeHandlerTestVariables.ElementsFixtureProvider,
			       	};
		}
	}

	public class GenericCollectionTypeHandlerTestUnit : GenericCollectionTypeHandlerTestUnitBase
	{
		public void TestSubQuery()
		{
			IQuery q = NewQuery(_helper.ItemType);
			IQuery qq = q.Descend(GenericCollectionTestFactory.FieldName);
			IConstraint constraint = qq.Constrain(FirstElement(_helper.Elements));
			IObjectSet set = qq.Execute();
			Assert.AreEqual(1, set.Count);

			_helper.AssertPlainContent((IEnumerable) set[0]);
		}

		public void TestRetrieve()
		{
			object item = RetrieveOnlyInstance(_helper.ItemType);
			_helper.AssertCollection(item);
		}

		public void TestDefrag()
		{
			Defragment();

			Object item = RetrieveOnlyInstance(_helper.ItemType);
			_helper.AssertCollection(item);
		}

		public virtual void TestFailingQuery()
		{
			AssertQuery(false, _helper.NotContained, false);
		}

		public virtual void TestSuccessfulContainsQuery()
		{
			AssertQuery(true, FirstElement(_helper.Elements), true);
		}

		public virtual void TestFailingContainsQuery()
		{
			AssertQuery(false, _helper.NotContained, true);
		}

		public virtual void TestCompareItems()
		{
			AssertCompareItems(FirstElement(_helper.Elements), true);
		}

		public virtual void TestFailingCompareItems()
		{
			AssertCompareItems(_helper.NotContained, false);
		}

		public virtual void TestDeletion()
		{
			AssertFirstClassElementCount(Count(_helper.Elements));
			object item = RetrieveOnlyInstance(_helper.ItemType);
			Db().Delete(item);
			Db().Purge();
			Db4oAssert.PersistedCount(0, _helper.ItemType);
			AssertFirstClassElementCount(0);
		}

		private static void AssertFirstClassElementCount(int expected)
		{
			if (!IsFirstClass(CollectionItemType()))
			{
				return;
			}
			Db4oAssert.PersistedCount(expected, CollectionItemType());
		}

		private static bool IsFirstClass(Type collectionItemType)
		{
			return typeof(GenericCollectionTypeHandlerTestVariables.FirstClassElement) == collectionItemType;
		}

		public virtual void TestActivation()
		{
			object item = RetrieveOnlyInstance(_helper.ItemType);
			ICollection coll = CollectionFor(item);
			Assert.AreEqual(Count(_helper.Elements), coll.Count);
			object element = FirstElement(coll);

			if (Db().IsActive(element))
			{
				Db().Deactivate(item, int.MaxValue);
				Assert.IsFalse(Db().IsActive(element));
				Db().Activate(item, int.MaxValue);
				Assert.IsTrue(Db().IsActive(element));
				
				Assert.AreEqual(Count(_helper.Elements), coll.Count);
			}
		}
	}
}
