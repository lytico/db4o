/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Linq.Tests
{
	public class VisualBasicTestCase : AbstractDb4oLinqTestCase
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
		}

		public void TestSimulateVisualBasic()
		{
			AssertQuery("(Person(Name == 'jb'))",
				delegate
				{
					var jbs = Db ().Cast<object> ()
								   .Select(o => (Person)o)
								   .Where(p => p.Name == "jb")
								   .Select(p => p);

					AssertSet(new[]
						{
							new Person { Name = "jb", Age = 24 },
							new Person { Name = "jb", Age = 7 },
							new Person { Name = "jb", Age = 28 },
							new Person { Name = "jb", Age = 34 },
						}, jbs);
				});
		}

		public void TestRetrieveAllObjects()
		{
			AssertQuery(null,
				delegate
				{
					var all = from object o in Db ()
							  select o;

					AssertSet(new[]
						{
							new Person { Name = "jb", Age = 24 },
							new Person { Name = "ana", Age = 20 },
							new Person { Name = "reg", Age = 25 },
							new Person { Name = "ro", Age = 32 },
							new Person { Name = "jb", Age = 7 },
							new Person { Name = "jb", Age = 28 },
							new Person { Name = "jb", Age = 34 },
						}, all);
				});
		}
	}
}
