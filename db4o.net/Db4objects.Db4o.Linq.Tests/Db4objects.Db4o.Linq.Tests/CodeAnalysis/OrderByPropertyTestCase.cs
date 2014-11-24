/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Linq.Tests.CodeAnalysis
{
	public class OrderByPropertyTestCase : AbstractDb4oLinqTestCase
	{
		public class Person
		{
			public string _name;
			public int _age;

			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}

			public int Age
			{
				get { return _age; }
				set { _age = value; }
			}

			public override bool Equals(object obj)
			{
				Person p = obj as Person;
				if (p == null) return false;

				return p._name == _name && p._age == _age;
			}

			public override int GetHashCode()
			{
				return _age ^ _name.GetHashCode();
			}
		}

		protected override void Store()
		{
			Store(new Person { Name = "Pedro", Age = 17 });
			Store(new Person { Name = "Superman", Age = 34 });
			Store(new Person { Name = "Pedro", Age = 38 });
			Store(new Person { Name = "Spiderman", Age = 24 });
		}

		public void TestOrderByProperty()
		{
			AssertQuery("(Person(_age)(_name))(orderby _name asc)(orderby _age asc)",
				delegate
				{
					var pedros = from Person p in Db()
								 orderby p.Name, p.Age
								 select p;

					AssertSequence(new[]
						{
							new Person { Name = "Pedro", Age = 17 },
							new Person { Name = "Pedro", Age = 38 },
							new Person { Name = "Spiderman", Age = 24 },
							new Person { Name = "Superman", Age = 34 },
						}, pedros);
				});
		}
	}
}
