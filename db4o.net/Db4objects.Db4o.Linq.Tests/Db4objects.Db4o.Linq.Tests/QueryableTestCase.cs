/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System.Linq;
using System.Linq.Expressions;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests
{
	public class QueryableTestCase : AbstractDb4oLinqTestCase
	{
#if !CF_3_5
		public class Person
		{
			public string Name;
			public int Age;

			public override int GetHashCode()
			{
				return Age ^ Name.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				var p = obj as Person;
				if (p == null) return false;

				return Name == p.Name && Age == p.Age;
			}
		}

		protected override void Store()
		{
			Store(new Person { Name = "jb", Age = 26 });
			Store(new Person { Name = "jb", Age = 28 });
			Store(new Person { Name = "ro", Age = 32 });
			Store(new Person { Name = "an", Age = 22 });
		}

		public void TestQueryableWhere()
		{
			AssertQuery ("(Person(Name == 'jb'))", () =>
			{
				var queryable = Db().AsQueryable<Person>().Where(p => p.Name == "jb");

				Assert.AreEqual("jb", queryable.First().Name);
			});
		}

		public void TestQueryableWhereOrderBy()
		{
			AssertQuery ("(Person(Age)(Name == 'jb'))(orderby Age asc)", () =>
			{
				var aggregate = Db().AsQueryable<Person>().Where(p => p.Name == "jb").OrderBy(p => p.Age).Aggregate(0, (i, p) => p.Age + i);

				Assert.AreEqual(26 + 28, aggregate);
			});
		}

		public void TestQueryableMixDb4oEnumerable()
		{
			AssertQuery ("(Person(Name == 'jb'))", () =>
			{
				bool b = Db().AsQueryable<Person>().Where(p => p.Name == "jb").Select(p => p.Age).OrderBy(i => (i % 2)).Where(i => i > 27).Any();

				Assert.IsTrue(b);
			});
		}
#endif
	}
}

