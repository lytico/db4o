/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Common.Migration;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class QueryingMigrationTestCase : HandlerUpdateTestCaseBase
	{
		private const int ObjectCount = 5;

		public class Car
		{
			public string _name;

			public QueryingMigrationTestCase.Pilot _pilot;
		}

		public class Pilot
		{
			public string _name;
		}

		protected override object[] CreateValues()
		{
			object[] cars = new object[ObjectCount];
			for (int i = 0; i < cars.Length; i++)
			{
				QueryingMigrationTestCase.Car car = new QueryingMigrationTestCase.Car();
				car._name = "Car " + i;
				QueryingMigrationTestCase.Pilot pilot = new QueryingMigrationTestCase.Pilot();
				car._pilot = pilot;
				pilot._name = "Pilot " + i;
				cars[i] = car;
			}
			return cars;
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
		}

		protected override object CreateArrays()
		{
			return null;
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
		}

		// do nothing
		protected override void AssertQueries(IExtObjectContainer objectContainer)
		{
			for (int i = 0; i < ObjectCount; i++)
			{
				IQuery query = objectContainer.Query();
				query.Constrain(typeof(QueryingMigrationTestCase.Car));
				query.Descend("_pilot").Descend("_name").Constrain("Pilot " + i);
				IObjectSet objectSet = query.Execute();
				Assert.AreEqual(1, objectSet.Count);
				QueryingMigrationTestCase.Car car = (QueryingMigrationTestCase.Car)objectSet.Next
					();
				Assert.AreEqual("Car " + i, car._name);
				Assert.AreEqual("Pilot " + i, car._pilot._name);
			}
		}

		protected override string TypeName()
		{
			return "querying";
		}
	}
}
