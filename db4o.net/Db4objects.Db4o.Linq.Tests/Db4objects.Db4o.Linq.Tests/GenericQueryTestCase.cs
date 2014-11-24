/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Linq;

using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

using Db4oUnit;
using Db4oUnit.Extensions;

#if !CF // csc fails to find S.R.FieldInfo.GetFieldFromHandle

namespace Db4objects.Db4o.Linq.Tests
{
	public class GenericQueryTestCase : AbstractDb4oLinqTestCase
	{
		public class Person
		{
			public string Name;
			public int Age;

			public override bool Equals(object obj)
			{
				Person p = obj as Person;
				if (p == null) return false;

				return p.Name == this.Name && p.Age == this.Age;
			}

			public override int GetHashCode()
			{
				return this.Age ^ this.Name.GetHashCode();
			}
		}

		protected override void Store()
		{
			Store(new Person { Name = "jb", Age = 24 });
			Store(new Person { Name = "ana", Age = 20 });
			Store(new Person { Name = "reg", Age = 25 });
			Store(new Person { Name = "ro", Age = 32 });
		}

		class PersonFinder<T> where T : Person
		{
			private IObjectContainer _database;

			public PersonFinder(IObjectContainer database)
			{
				_database = database;
			}

			public T GetPerson(string name)
			{
				return (from T person in _database
						where person.Name == name
						select person).FirstOrDefault ();
			}
		}

		public void TestGenericQuery()
		{
			AssertQuery("(Person(Name == 'ana'))",
				delegate
				{
					var finder = new PersonFinder<Person>(Db());
					var ana = finder.GetPerson("ana");

					Assert.IsNotNull(ana);
					Assert.AreEqual("ana", ana.Name);
				});
		}
	}
}

#endif
