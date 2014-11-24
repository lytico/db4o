/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests.Queries
{
	public class QueryPrettyPrinterTestCase : AbstractDb4oTestCase
	{
        

		public class Person
		{
			public int Age;
			public string Name;
			public Person Parent;
		}

        protected override void Db4oSetupAfterStore()
        {
            Container().ProduceClassMetadata(ReflectClass(typeof(Person)));
        }

		public void TestDoubleDescending()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));
			query.Descend("Parent").Descend("Age").Constrain(60).Greater();
			AssertQueryString("(Person(Parent(Age > 60)))", query);
		}

		public void TestAndDoubleDescending()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));
			query.Descend("Parent").Descend("Age").Constrain(60).Greater().And(query.Descend("Parent").Descend("Name").Constrain("jb"));
			AssertQueryString("(Person(Parent((Name == 'jb') and (Age > 60))))", query);
		}


		public void TestConstrainOnType()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			AssertQueryString("(Person)", query);
		}

		public void TestConstrainEqualsOnField()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));
			query.Descend("Name").Constrain("jb");

			AssertQueryString("(Person(Name == 'jb'))", query);
		}

		public void TestConstrainLessThanOnField()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));
			query.Descend("Age").Constrain(12).Smaller();

			AssertQueryString("(Person(Age < 12))", query);
		}

		public void TestAndContraints()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Age").Constrain(22).Greater().And(query.Descend("Name").Constrain("jb"));

			AssertQueryString("(Person((Name == 'jb') and (Age > 22)))", query);
		}

		public void TestOrContraints()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Age").Constrain(22).Greater().Equal().Or(query.Descend("Name").Constrain("ro"));

			AssertQueryString("(Person((Name == 'ro') or (Age >= 22)))", query);
		}

		public void TestComplexConstraint()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Age").Constrain(22).Greater().Equal().And(query.Descend("Age").Constrain("32").Smaller().Equal()).Or(query.Descend("Name").Constrain("jb"));

			AssertQueryString("(Person((Name == 'jb') or ((Age <= 32) and (Age >= 22))))", query);
		}

		public void TestNot()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Name").Constrain("jb").Not();

			AssertQueryString("(Person(Name not 'jb'))", query);
		}

		public void TestStartsWith()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Name").Constrain("jb").StartsWith(true);

			AssertQueryString("(Person(Name startswith 'jb'))", query);
		}

		public void TestEndsWith()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Name").Constrain("jb").EndsWith(true);

			AssertQueryString("(Person(Name endswith 'jb'))", query);
		}

		public void TestContains()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Name").Constrain("jb").Contains();

			AssertQueryString("(Person(Name contains 'jb'))", query);
		}

		public void TestOrderByAscending()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Age").OrderAscending();

			AssertQueryString("(Person(Age))(orderby Age asc)", query);
		}

		public void TestOrderByAscendingGreaterOnSameField()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));
			
			query.Descend("Age").Constrain(21).Greater();
			query.Descend("Age").OrderAscending();

			AssertQueryString("(Person(Age > 21))(orderby Age asc)", query);
		}

		public void TestOrderByDescending()
		{
			var query = CreateQuery();
			query.Constrain(typeof(Person));

			query.Descend("Age").OrderDescending();

			AssertQueryString("(Person(Age))(orderby Age desc)", query);
		}

		private IQuery CreateQuery()
		{
			return Db().Query();
		}

		private static void AssertQueryString(string expected, IQuery query)
		{
			Assert.AreEqual(expected, query.ToQueryString());
		}
	}
}
