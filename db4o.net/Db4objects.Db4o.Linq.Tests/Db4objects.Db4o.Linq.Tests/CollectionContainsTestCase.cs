/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq.Tests
{
	public class CollectionContainsTestCase : AbstractDb4oLinqTestCase
	{
		public abstract class Person
		{
			protected abstract IEnumerable<string> GetNames();

			public override bool Equals(object obj)
			{
				Person p = obj as Person;
				if (p == null) return false;

				var anames = GetNames();
				var bnames = p.GetNames();

				if (anames == null)
					return bnames == null;

				return anames.SequenceEqual(bnames);
			}

			public override int GetHashCode()
			{
				var names = GetNames();
				if (names == null) return 0;

				var list = names.ToList();

				if (list.Count == 0) return 0;

				int hash = list[0].GetHashCode();
				for (int i = 1; i < list.Count; i++)
					hash ^= list[i].GetHashCode();

				return hash;
			}
		}

		public class ArrayListPerson : Person
		{
			public IList Names = new ArrayList();

			protected override IEnumerable<string> GetNames()
			{
				return Names.Cast<string>();
			}
		}

		public class ArrayPerson : Person
		{
			public string[] Names;

			protected override IEnumerable<string> GetNames()
			{
				return Names;
			}
		}

		public static ArrayListPerson[] PeopleWithArrayList()
		{
			return new[]
			       	{
			       		new ArrayListPerson {Names = {"Biro", "Biro"}},
			       		new ArrayListPerson {Names = {"Luna"}},
			       		new ArrayListPerson {Names = {"Loustic"}},
			       		new ArrayListPerson {Names = {"Loupiot"}},
			       		new ArrayListPerson {Names = {"Biro", "Miro"}},
			       		new ArrayListPerson {Names = {"Tounage"}}
			       	};
		}


		protected override void Store()
		{
			foreach (var person in PeopleWithArrayList())
			{
				Store(person);
			}

			foreach (var person in PeopleWithArray())
			{
				Store(person);
			}
		}

		public void TestQueryOnArrayListContains()
		{
			var q = NewQuery(typeof(ArrayListPerson));
			q.Descend("Names").Constrain("Biro").Contains();

			var persons = new ObjectSetWrapper<Person>(q.Execute());

			AssertSet(new[]
				{
					new ArrayListPerson { Names = { "Biro", "Biro" } },
					new ArrayListPerson { Names = { "Biro", "Miro" } },
				}, persons);
		}

		public void TestLinqQueryOnArrayListContains()
		{
			AssertQuery(
				from ArrayListPerson p in Db()
				where p.Names.Contains("Biro")
				select p,
			
				"(ArrayListPerson(Names == 'Biro'))",

				from ArrayListPerson p in PeopleWithArrayList()
				where p.Names.Contains("Biro")
				select p);
		}

		public void TestLinqQueryOnArrayListNotContains()
		{
			AssertQuery(
				from ArrayListPerson p in Db()
				where !p.Names.Contains("Biro")
				select p,

				"(ArrayListPerson(Names not 'Biro'))",

				from ArrayListPerson p in PeopleWithArrayList()
				where !p.Names.Contains("Biro")
				select p);
		}



		public void TestQueryOnArrayContains()
		{
			var q = NewQuery(typeof(ArrayPerson));
			q.Descend("Names").Constrain("Biro");

			var persons = new ObjectSetWrapper<Person>(q.Execute());

			AssertSet(new[]
				{
					new ArrayPerson { Names = new [] { "Biro", "Biro" } },
					new ArrayPerson { Names = new [] { "Biro", "Miro" } },
				}, persons);
		}

#if !CF // It fails with a TypeLoadException on CF. 

		public void TestLinqQueryOnArrayContains()
		{
			AssertQuery("(ArrayPerson(Names == 'Biro'))",
				delegate
				{
					var biros = from ArrayPerson p in Db()
								where p.Names.Contains("Biro")
								select p;

					AssertSet(new[]
						{
							new ArrayPerson { Names = new [] { "Biro", "Biro" } },
							new ArrayPerson { Names = new [] { "Biro", "Miro" } },
						}, biros);
				});
		}
		
		public void TestLinqQueryOnArrayNotContains()
		{
			AssertQuery(
				from ArrayPerson p in Db()
				where !p.Names.Contains("Biro")
				select p,

				"(ArrayPerson(Names not 'Biro'))",

				from ArrayPerson p in PeopleWithArray()
				where !p.Names.Contains("Biro")
				select p);
		}
#endif

		private static IEnumerable<ArrayPerson> PeopleWithArray()
		{
			foreach (var person in PeopleWithArrayList())
			{
				yield return new ArrayPerson { Names = NamesFrom(person) };
			}
		}
		
		private static string[] NamesFrom(ArrayListPerson person)
		{
			string[] names = new string[person.Names.Count];
			person.Names.CopyTo(names, 0);
			return names;
		}
	}
}
