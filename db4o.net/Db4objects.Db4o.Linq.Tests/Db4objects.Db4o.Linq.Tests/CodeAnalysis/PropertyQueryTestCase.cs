/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Linq.Tests.CodeAnalysis
{
	public class PropertyQueryTestCase : AbstractDb4oLinqTestCase
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

		public void TestSimpleProperty()
		{
			AssertQuery("(Person(_name == 'Pedro'))",
				delegate
				{
					var pedros = from Person p in Db()
								 where p.Name == "Pedro"
								 select p;

					AssertSet(new[]
						{
							new Person { Name = "Pedro", Age = 17 },
							new Person { Name = "Pedro", Age = 38 },
						}, pedros);
				});
		}
	}
}
