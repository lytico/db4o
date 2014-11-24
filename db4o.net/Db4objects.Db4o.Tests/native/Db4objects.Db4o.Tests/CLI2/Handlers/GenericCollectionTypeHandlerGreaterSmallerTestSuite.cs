/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */
using System;
using Db4objects.Db4o.Query;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
	public class GenericCollectionTypeHandlerGreaterSmallerTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			IFixtureProvider ElementsFixtureProvider = new SimpleFixtureProvider(
				GenericCollectionTypeHandlerTestVariables.ElementSpec,
				new object[]
				{
					GenericCollectionTypeHandlerTestVariables.StringElementSpec,
					GenericCollectionTypeHandlerTestVariables.IntElementSpec,
					GenericCollectionTypeHandlerTestVariables.NullableIntElementSpec,
				});

			return new IFixtureProvider[]
					{
			       		new Db4oFixtureProvider(),
			       		GenericCollectionTypeHandlerTestVariables.CollectionFixtureProvider,
			       		ElementsFixtureProvider
			       	};
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(GenericCollectionTypeHandlerGreaterSmallerTestUnit) };
		}

		public class GenericCollectionTypeHandlerGreaterSmallerTestUnit : GenericCollectionTypeHandlerTestUnitBase
		{
			public virtual void TestSuccessfulSmallerQuery()
			{
				IQuery q = NewQuery(_helper.ItemType);
				q.Descend(GenericCollectionTestFactory.FieldName).Constrain(_helper.LargeElement).Smaller();
				AssertQueryResult(q, true);
			}

			public virtual void TestFailingGreaterQuery()
			{
				IQuery q = NewQuery(_helper.ItemType);
				q.Descend(GenericCollectionTestFactory.FieldName).Constrain(_helper.LargeElement).Greater();
				AssertQueryResult(q, false);
			}
		}
	}
}
