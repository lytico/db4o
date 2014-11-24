/* Copyright (C) 2007 - 2010  Versant Inc.  http://www.db4o.com */
using System.Linq;
using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests
{
	public class UntypedQueryTestCase : AbstractDb4oLinqTestCase
	{
		private static object[] People = {
											new Person("adriano"),
											new Person("gislene"),
											new Person("carolina"),
										 };

		protected override void Store()
		{
			foreach (Person person in People)
			{
				Store(person);
			}
		}

		public void TestWhere()
		{
			AssertQuery(
							from object person in Db()
							where person.ToString().Contains("(gislene)")
							select person,
							
							null,

							from object p in People
							where p.ToString().Contains("(gislene)")
							select p);
		}

		public void TestOrderBy()
		{
			AssertQuery(
				from object person in Db()
				orderby person.ToString()
				select person,

				null,

				from object person in People
				orderby person.ToString()
				select person);
		}

		public void TestOrderByDescending()
		{
			AssertQuery(
				from object person in Db()
				orderby person.ToString() descending 
				select person,

				null,

				from object person in People
				orderby person.ToString() descending 
				select person);
		}

		public void TestThenBy()
		{
			AssertQuery(
				from object person in Db()
				orderby person.ToString(), person.ToString().GetHashCode()
				select person,

				null,

				from object person in People
				orderby person.ToString(), person.ToString().GetHashCode()
				select person);
		}
	}

	public class Person
	{
		public string Name;

		public Person(string name)
		{
			Name = name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			Person other = obj as Person;
			if (other == null) return false;

			return other.Name == Name;
		}

		public override string ToString()
		{
			return "Person(" + Name + ")";
		}
	}
}
