/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class MapTypeHandlerTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new Db4oFixtureProvider(), MapTypeHandlerTestVariables
				.MapFixtureProvider, MapTypeHandlerTestVariables.MapKeysProvider, MapTypeHandlerTestVariables
				.MapValuesProvider, MapTypeHandlerTestVariables.TypehandlerFixtureProvider };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(MapTypeHandlerTestSuite.MapTypeHandlerUnitTestCase) };
		}

		public class MapTypeHandlerUnitTestCase : CollectionTypeHandlerUnitTest
		{
			protected override void FillItem(object item)
			{
				FillMapItem(item);
			}

			protected override void AssertContent(object item)
			{
				AssertMapContent(item);
			}

			protected override void AssertPlainContent(object item)
			{
				AssertPlainMapContent((IDictionary)item);
			}

			protected override AbstractItemFactory ItemFactory()
			{
				return (AbstractItemFactory)MapTypeHandlerTestVariables.MapImplementation.Value;
			}

			protected override ITypeHandler4 TypeHandler()
			{
				return (ITypeHandler4)MapTypeHandlerTestVariables.MapTypehander.Value;
			}

			protected override ListTypeHandlerTestElementsSpec ElementsSpec()
			{
				return (ListTypeHandlerTestElementsSpec)MapTypeHandlerTestVariables.MapKeysSpec.Value;
			}

			protected override void AssertCompareItems(object element, bool successful)
			{
				IQuery q = NewQuery();
				object item = ItemFactory().NewItem();
				IDictionary map = MapFromItem(item);
				map[element] = Values()[0];
				q.Constrain(item);
				AssertQueryResult(q, successful);
			}
		}
	}
}
