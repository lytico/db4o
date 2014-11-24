/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
using System.Collections.Generic;
using System.Linq;

namespace Db4objects.Db4o.Linq.Tests
{
	public class CollectionContainsObjectTestCase : AbstractDb4oLinqTestCase
	{
	    public class Role
		{
			public Role(string id)
			{
				_id = id;
			}

			public string Id
			{
				get { return _id; }
			}

			public override string ToString()
			{
				return _id;
			}

			public override bool Equals(object obj)
			{
				Role other = obj as Role;
				if (other == null) return false;

				return other.Id == Id;
			}

			public override int GetHashCode()
			{
				return Id.GetHashCode();
			}
			
			public string _id;
		}

		abstract public class Person
		{
			protected abstract IEnumerable<Role> GetRoles();

			public override string ToString()
			{
				string result = GetRoles().Aggregate("Person(", (acc, role) => acc + role + ", ") + "/";
				return result.Replace(", /", ")");
			}

			public override bool Equals(object obj)
			{
				Person other = obj as Person;
				if (other == null) return false;

				return GetRoles().Except(other.GetRoles()).Count() == 0;
			}

			public override int GetHashCode()
			{
				return GetRoles().Aggregate(0, (acc, role) => acc ^ role.GetHashCode());
			}
		}

		public class IListPerson : Person
		{
			public IList<Role> Roles;

			public IListPerson(IList<Role> roles)
			{
				Roles = roles;
			}

			protected override IEnumerable<Role> GetRoles()
			{
				return Roles;
			}
		}

		public class ArrayPerson : Person
		{
			public Role[] Roles;

			public ArrayPerson(Role[] roles)
			{
				Roles = roles;
			}

			protected override IEnumerable<Role> GetRoles()
			{
				return Roles;
			}
		}

		private static Role[] Roles = new[]
		                              	{
		                              		new Role("Employe"), 
											new Role("Boss"), 
											new Role("The boss"),
		                              	};

		private static IListPerson[] PeopleWithIList()
		{
			return new[]
			       	{
			       		NewIListPerson(Roles[0]),
			       		NewIListPerson(Roles[0], Roles[1]),
			       		NewIListPerson(Roles[2]),
			       		NewIListPerson(Roles[1]),
			       	};
		}

		private static IEnumerable<ArrayPerson> PeopleWithArray()
		{
			foreach (var person in PeopleWithIList())
			{
				yield return new ArrayPerson(person.Roles.ToArray());
			}
		}
		
		private static IListPerson NewIListPerson(params Role[] roles)
		{
			return new IListPerson(roles);
		}

		protected override void Store()
		{
			foreach (var person in PeopleWithIList())
			{
				Store(person);
			}
			
			foreach (var person in PeopleWithArray())
			{
				Store(person);
			}
		}

		public void TestIListPersonContains()
		{
			var role = RoleFromDb(Roles[1]);

			AssertQuery(
				from IListPerson p in Db()
				where p.Roles.Contains(role)
				select p,
				
				"(IListPerson(Roles == " + role + "))",
				
				from IListPerson p in PeopleWithIList()
				where p.Roles.Contains(role)
				select p);
		}

		public void TestIListPersonNotContains()
		{
			var role = RoleFromDb(Roles[1]);
			
			AssertQuery(
				from IListPerson p in Db()
				where !p.Roles.Contains(role)
				select p,

				"(IListPerson(Roles not " + role + "))",

				from IListPerson p in PeopleWithIList()
				where !p.Roles.Contains(role)
				select p);
		}

#if !CF // It fails with a TypeLoadException on CF. 
		public void TestArrayPersonContains()
		{
			var role = RoleFromDb(Roles[1]);

			AssertQuery(
				from ArrayPerson p in Db()
				where p.Roles.Contains(role)
				select p,

				"(ArrayPerson(Roles == " + role + "))",

				from ArrayPerson p in PeopleWithArray()
				where p.Roles.Contains(role)
				select p);
		}

		public void TestArrayPersonNotContains()
		{
			var role = RoleFromDb(Roles[1]);

			AssertQuery(
				from ArrayPerson p in Db()
				where !p.Roles.Contains(role)
				select p,

				"(ArrayPerson(Roles not " + role + "))",

				from ArrayPerson p in PeopleWithArray()
				where !p.Roles.Contains(role)
				select p);
		}
#endif

		private Role RoleFromDb(Role role)
		{
			return (from Role candidate in Db()
					where candidate.Id == role.Id
					select candidate).Single();
		}
	}
}
