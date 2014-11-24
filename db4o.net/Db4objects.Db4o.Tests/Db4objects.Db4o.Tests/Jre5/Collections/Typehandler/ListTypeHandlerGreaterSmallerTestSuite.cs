/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class ListTypeHandlerGreaterSmallerTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			ListTypeHandlerTestElementsSpec[] elementSpecs = new ListTypeHandlerTestElementsSpec
				[] { ListTypeHandlerTestVariables.StringElementsSpec, ListTypeHandlerTestVariables
				.IntElementsSpec };
			return new IFixtureProvider[] { new Db4oFixtureProvider(), ListTypeHandlerTestVariables
				.ListFixtureProvider, new SimpleFixtureProvider(ListTypeHandlerTestVariables.ElementsSpec
				, (object[])elementSpecs), ListTypeHandlerTestVariables.TypehandlerFixtureProvider
				 };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(ListTypeHandlerGreaterSmallerTestSuite.ListTypeHandlerGreaterSmallerTestUnit
				) };
		}

		public class ListTypeHandlerGreaterSmallerTestUnit : ListTypeHandlerTestUnitBase
		{
			/// <exception cref="System.Exception"></exception>
			public virtual void TestSuccessfulSmallerQuery()
			{
				IQuery q = NewQuery(ItemFactory().ItemClass());
				q.Descend(AbstractItemFactory.ListFieldName).Constrain(LargeElement()).Smaller();
				AssertQueryResult(q, true);
			}

			/// <exception cref="System.Exception"></exception>
			public virtual void TestFailingGreaterQuery()
			{
				IQuery q = NewQuery(ItemFactory().ItemClass());
				q.Descend(AbstractItemFactory.ListFieldName).Constrain(LargeElement()).Greater();
				AssertQueryResult(q, false);
			}
		}
	}
}
