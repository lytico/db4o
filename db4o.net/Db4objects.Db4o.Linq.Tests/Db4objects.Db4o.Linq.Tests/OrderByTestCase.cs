/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Db4objects.Db4o.Linq.Tests
{
	public class OrderByTestCase : AbstractDb4oLinqTestCase
	{
		public class Person
		{
			public string Name;
			public int Age;
			public Person Parent;
			public Guid Id;

			public int UnoptimizableAgeProperty
			{
				get { return Age + 1; }
			}

			public string UnoptimizableNameProperty
			{
				get
				{
					if (string.IsNullOrEmpty(Name))
					{
						return Age.ToString();
					}
					return Name + " (" + Age + ")";
				}
			}

			public string OptimizableNameProperty
			{
				get { return Name; }
			}

			public int OptimizableAgeProperty
			{
				get { return Age; }
			}

			public override bool Equals(object obj)
			{
				Person p = obj as Person;
				if (p == null) return false;

				return p.Name == Name && p.Age == Age && p.Id == Id;
			}

			public override int GetHashCode()
			{
				return Age ^ Name.GetHashCode() ^ Id.GetHashCode();
			}

			public override string ToString()
			{
				return "Person(" + Name + ", " + Age + ")";
			}

			public int GetAge()
			{
				return Age;
			}

			public string GetName()
			{
				return Name;
			}
		}

		protected override void Store()
		{
			foreach (var person in People())
			{
				Store(person);
			}
		}

		private static Person[] People()
		{
			return new[] {
			             	new Person { Name = "jb", Age = 24 , Id =  NewGuid(1) },
			             	new Person { Name = "ana", Age = 24, Id =  NewGuid(2) },
			             	new Person { Name = "reg", Age = 25, Id =  NewGuid(3)},
			             	new Person { Name = "ro", Age = 25, Id =  NewGuid(4) },
			             	new Person { Name = "jb", Age = 7, Id =  NewGuid(5) }
			             };
		}

		private static Guid NewGuid(byte value)
		{
			return new Guid(value, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}

		public void _TestUnoptimizableThenByOnOptimizedOrderBy()
		{
			var query = from Person p in Db()
						orderby p.OptimizableAgeProperty ascending,
							p.UnoptimizableNameProperty descending
						select p;
			AssertOrderByNameDescAgeAsc("(Person)(orderby Age asc)", query);
		}

		public void TestUnoptimizableOrderByAscendingThenDescendingOnProperties()
		{
			var query = from Person p in Db()
						orderby p.UnoptimizableAgeProperty ascending,
							p.UnoptimizableNameProperty descending
						select p;

			AssertOrderByNameDescAgeAsc("(Person)", query);
		}

		public void TestSimpleOrderByAscendingThenDescendingProperties()
		{
			var query = from Person p in Db()
						orderby p.OptimizableAgeProperty ascending,
							p.OptimizableNameProperty descending
						select p;

			AssertOrderByNameDescAgeAsc(query);
		}

		public void TestSimpleOrderByAscendingThenDescendingMethods()
		{
			var query = from Person p in Db()
						orderby p.GetAge() ascending,
							p.GetName() descending
						select p;

			AssertOrderByNameDescAgeAsc(query);
		}

		public void TestSimpleOrderByAscendingThenDescendingFields()
		{
			var query = from Person p in Db()
					  orderby p.Age ascending, p.Name descending
					  select p;

			AssertOrderByNameDescAgeAsc(query);
		}

		private void AssertOrderByNameDescAgeAsc(IDb4oLinqQuery<Person> query)
		{
			string expectedQuery = "(Person(Name)(Age))(orderby Age asc)(orderby Name desc)";
			AssertOrderByNameDescAgeAsc(expectedQuery, query);
		}

		private void AssertOrderByNameDescAgeAsc(string expectedQuery, IDb4oLinqQuery<Person> query)
		{
			AssertQuerySequence(
				query, 
				expectedQuery, 

				from p in People()
				orderby p.Age ascending, p.Name descending
				select p);
		}

#if !CF
		public void TestOrderByValueType()
		{
			AssertQuerySequence("(Person(Id))(orderby Id asc)",
				queryable => from p in queryable
							 orderby p.Id
							 select p);
		}

		public void TestOrderByOnUnoptimizableStringProperty()
		{
			AssertQuerySequence(
				"(Person(Name not 'jb'))",

				queryable => from p in queryable
							 where p.Name != "jb"
							 orderby p.UnoptimizableNameProperty
							 select p);
		}

		public void TestOrderByOnUnoptimizableProperty()
		{
			AssertQuerySequence(
				"(Person(Name == 'jb'))",

				queryable => from p in queryable
							 where p.Name == "jb"
							 orderby p.UnoptimizableAgeProperty
							 select p);
		}

		public void TestOrderByDescendingOnWhere()
		{
			AssertQuerySequence(
				"(Person(Age)(Name == 'jb'))(orderby Age desc)",

				queryable => from p in queryable
							 where p.Name == "jb"
							 orderby p.Age descending
							 select p);
		}

		public void TestOrderByDescendingOnUnoptimizableProperty()
		{
			AssertQuerySequence(
				"(Person(Name == 'jb'))",

				queryable => from p in queryable
							 where p.Name == "jb"
							 orderby p.UnoptimizableAgeProperty descending
							 select p);
		}

		public void TestSimpleOrderByDescendingThenAscending()
		{
			AssertQuerySequence(
				"(Person(Name)(Age))(orderby Age desc)(orderby Name asc)",

				queryable => from p in queryable
							 orderby p.Age descending, p.Name ascending
							 select p);
		}

		private void AssertQuerySequence(string expectedQueryString, Func<IQueryable<Person>, IEnumerable<Person>> query)
		{
			AssertQuerySequence(
				query(Db().Cast<Person>().AsQueryable()),

				expectedQueryString,

				query(People().AsQueryable()));
		}
#endif

		public void TestOrderByDescendingThenAscendingOnCompositeFieldAccess()
		{
			AssertQueryTranslation(
				"(Person(Parent(Name)(Age)))(orderby Parent.Age desc)(orderby Parent.Name asc)",
				from Person p in Db()
				orderby p.Parent.Age descending, p.Parent.Name ascending
				select p);
		}


		private void AssertQueryTranslation<T>(string expectedRepresentation, IEnumerable<T> query)
		{
			AssertQuery(expectedRepresentation, () => query.ToList());
		}
		
		private void AssertQuerySequence<T>(IEnumerable<T> query, string expectedQueryString, IEnumerable<T> expectedSequence)
		{
			AssertQuery(query, expectedQueryString, actualSequence => AssertSequence(expectedSequence, actualSequence));
		}
	}
}
