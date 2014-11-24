/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System.Linq;
using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests
{
	public class PartiallyOptimizedQueryTestCase : AbstractDb4oLinqTestCase
	{
		public class Person
		{
			public string Name;
			public int Age;

			public override bool Equals(object obj)
			{
				Person p = obj as Person;
				if (p == null) return false;

				return p.Name == Name && p.Age == Age;
			}

			public override int GetHashCode()
			{
				return Age ^ Name.GetHashCode();
			}
		}

		protected override void Store()
		{
			Store(new Person { Name = "jb", Age = 24 });
			Store(new Person { Name = "ana", Age = 20 });
			Store(new Person { Name = "reg", Age = 25 });
			Store(new Person { Name = "ro", Age = 32 });
			Store(new Person { Name = "jb", Age = 7 });
			Store(new Person { Name = "jb", Age = 28 });
			Store(new Person { Name = "jb", Age = 34 });
			Store(new Person { Name = "alice", Age = 35 });
		}

		public void TestProjection()
		{
			AssertQuery("(Person(Name == 'jb'))",
				delegate
				{
					var peoples = from Person p in Db()
								  where p.Name == "jb"
								  select new { p.Age };

					var ages = peoples.ToList().Select(t => t.Age);

					AssertSet(new[] { 24, 7, 28, 34 }, ages);
				});
		}

		public void TestUnoptimizableWhere()
		{
			AssertQuery("(Person)",
				delegate
				{
					var a = from Person p in Db()
							 where p.Name[0] == 'a'
							 select p;

					AssertSet(new[]
						{
							new Person { Name = "ana", Age = 20 },
							new Person { Name = "alice", Age = 35 }
						}, a);
				});
		}

		public void TestLet()
		{
			AssertQuery("(Person)",
				delegate
				{
					var ages = from Person p in Db()
							   let uname = p.Name.ToUpper()
							   where uname == "JB"
							   select p;

					AssertSet(new[]
						{
							new Person { Name = "jb", Age = 24 },
							new Person { Name = "jb", Age = 7 },
							new Person { Name = "jb", Age = 28 },
							new Person { Name = "jb", Age = 34 },
						}, ages);
				});
		}

		public void TestAverage()
		{
			AssertQuery("(Person(Name == 'jb'))",
				delegate
				{
					var ages = from Person p in Db()
							  where p.Name == "jb"
							  select p.Age;

					Assert.AreEqual(23.25, ages.Average());
				});
		}
	}
}
