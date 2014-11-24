/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System.Collections.Generic;
using System.Linq;

namespace Db4objects.Db4o.Linq.Tests
{
	public class WhereTestCase : AbstractDb4oLinqTestCase
	{
		public class Person
		{
			public string Name;
			public int Age;
			public bool IsFriend;

			public override bool Equals(object obj)
			{
				Person p = obj as Person;
				if (p == null) return false;

				return p.Name == Name && p.Age == Age && (p.IsFriend == IsFriend);
			}

			public override int GetHashCode()
			{
				return Age ^ Name.GetHashCode() ^ IsFriend.GetHashCode();
			}

			public bool	GetIsFriend()
			{
				return IsFriend;
			}

			public Person Self()
			{
				return this;
			}
		}

		public class Thing
		{
			public Person Owner;

			public override bool Equals(object obj)
			{
				Thing t = obj as Thing;
				if (t == null) return false;

				return t.Owner.Equals(Owner);
			}

			public override int GetHashCode()
			{
				return Owner.GetHashCode();
			}

			public Person GetOwner()
			{
				return Owner;		
			}
		}

		protected IEnumerable<Person> People()
		{
			return new[]
					{
						new Person {Name = "jb", Age = 24, IsFriend = true},
						new Person {Name = "ana", Age = 20, IsFriend = false},
						new Person {Name = "reg", Age = 25, IsFriend = false},
						new Person {Name = "ro", Age = 32, IsFriend = false},
						new Person {Name = "jb", Age = 7, IsFriend = true},
						new Person {Name = "jb", Age = 28, IsFriend = true},
						new Person {Name = "jb", Age = 34, IsFriend = true}
					};
			
		}

		protected IEnumerable<Thing> Things(IList<Person> people)
		{
			return new []
			       	{ 
						new Thing { Owner = people[0] },
						new Thing { Owner = people[1] },
						new Thing { Owner = people[2] },
						new Thing { Owner = people[3] }
					};
		}

		protected override void Store()
		{
			List<Person> people = People().ToList();
			foreach (var person in people)
			{
				Store(person);
			}

			foreach (var thing in Things(people))
			{
				Store(thing);	
			}
		}

		public void TestEqualsInWhere()
		{
			AssertQuery("(Person(Name == 'jb'))",
				delegate
				{
					var jbs = from Person p in Db()
							  where p.Name == "jb"
							  select p;

					AssertSet(People().Where(p => p.Name == "jb"), jbs);
				});
		}

		public void TestInversedEqualsInWhere()
		{
			AssertQuery("(Person(Name == 'jb'))",
				delegate
				{
					var jbs = from Person p in Db()
							  where "jb" == p.Name
							  select p;

					AssertSet(People().Where(p => p.Name == "jb"), jbs);
				});
		}

		public void TestLessThanInWhere()
		{
			AssertQuery("(Person(Age < 25))",
				delegate
				{
					var youngs = from Person p in Db()
								 where p.Age < 25
								 select p;

					AssertSet(People().Where(p => p.Age < 25), youngs);
				});
		}

		public void TestSimpleAnd()
		{
			AssertQuery(from Person p1 in Db()
			            where p1.Name == "jb" && p1.Age > 10 && p1.Age < 30
			            select p1.Age, 
						
						"(Person(((Name == 'jb') and (Age > 10)) and (Age < 30)))", 
						
						new[] { 24, 28 });
		}

		public void TestSimpleAndOnMultipleDescend()
		{
			AssertQuery(from Thing t in Db()
						where t.Owner.Name == "jb" && t.Owner.Age > 10 && t.Owner.Age < 30
						select t.Owner.Age,

						@"(Thing(Owner(((Name == 'jb') and (Age > 10)) and (Age < 30))))",

						new[] { 24 });
		}

		public void TestSimpleOr()
		{
			AssertQuery("(Person((Age < 10) or (Age > 30)))",
				delegate
				{
					var ages = from Person p in Db()
							   where p.Age < 10 || p.Age > 30
							   select p.Age;

					AssertSet(new[] { 7, 32, 34 }, ages);
				});
		}

		public void TestContainsCallsOnMultipleDescend()
		{
			AssertQuery(from Thing t in Db()
						where t.Owner.Name.Contains("jb") || t.Owner.Name.Contains("na")
						select t,

						@"(Thing(Owner((Name contains 'jb') or (Name contains 'na'))))",

						from t in Things(People().ToList())
						where t.Owner.Name.Contains("jb") || t.Owner.Name.Contains("na")
						select t);
		}

		public void TestNotMethodCall()
		{
			AssertQuery("(Person(IsFriend == False))",
				delegate
				{
					var notFriends = from Person p in Db()
									 where !p.GetIsFriend()
									 select p;

					AssertSet(
						People().Where(p => !p.GetIsFriend()),
						notFriends);
				});
		}

		public void TestNotMethodCallOnProperty()
		{
			AssertQuery(from Thing t in Db()
						where !t.Owner.GetIsFriend()
						select t,

						"(Thing(Owner(IsFriend == False)))",

						Things(People().ToList()).Where(t => !t.Owner.GetIsFriend()).ToArray());

		}

		public void TestNotMethodCallChain()
		{
			AssertQuery(from Thing t in Db()
						where !t.GetOwner().GetIsFriend()
						select t,

						"(Thing(Owner(IsFriend == False)))",

						Things(People().ToList()).Where(t => !t.Owner.GetIsFriend()).ToArray());

		}

		public void TestSimpleBoolean()
		{
			AssertQuery(
				from Person p in Db()
				where p.IsFriend
				select p,
				
				"(Person(IsFriend == True))",
				
				from p in People()
				where p.IsFriend
				select p);	
		}

		public void TestSimpleLocigalWithBooleanToTheRight()
		{
			AssertQuery(
				from Person p in Db()
				where p.Name == "jb" && p.IsFriend
				select p,

				"(Person((Name == 'jb') and (IsFriend == True)))",

				from p in People()
				where p.Name == "jb" && p.IsFriend
				select p);
		}

		public void TestSimpleLocigalWithBooleanToTheLeft()
		{
			AssertQuery(
				from Person p in Db()
				where p.IsFriend && p.Name == "jb"
				select p,

				"(Person((IsFriend == True) and (Name == 'jb')))",

				from p in People()
				where p.IsFriend && p.Name == "jb"
				select p);
		}

		public void TestFullBooleanToTheLeft()
		{
			AssertQuery(
				from Person p in Db()
				where true == p.IsFriend
				select p,

				"(Person(IsFriend == True))",

				from p in People()
				where true == p.IsFriend
				select p);
		}

		public void TestFullBooleanToTheRight()
		{
			AssertQuery(
				from Person p in Db()
				where p.IsFriend == true
				select p,

				"(Person(IsFriend == True))",

				from p in People()
				where p.IsFriend == true
				select p);
		}

		public void TestNotBoolean()
		{
			AssertQuery("(Person(IsFriend == False))",
				delegate
				{
					var notFriends = from Person p in Db()
									 where !p.IsFriend
									 select p;

					AssertSet(
						People().Where(p => !p.IsFriend),
						notFriends);
				});
		}

		public void TestSimpleNot()
		{
			AssertQuery("(Person(Name not 'jb'))",
				delegate
				{
					var notjb = from Person p in Db()
								where !(p.Name == "jb")
								select p;

					AssertSet(new[]
					{
						new Person { Name = "ana", Age = 20 },
						new Person { Name = "reg", Age = 25 },
						new Person { Name = "ro", Age = 32 },
					}, notjb);
				});
		}

		public void TestSimpleDifferent()
		{
			AssertQuery("(Person(Name not 'jb'))",
				delegate
				{
					var notjb = from Person p in Db()
								where p.Name != "jb"
								select p;

					AssertSet(new[]
						{
							new Person { Name = "ana", Age = 20 },
							new Person { Name = "reg", Age = 25 },
							new Person { Name = "ro", Age = 32 },
						}, notjb);
				});
		}

		public void TestConvolutedConditionals()
		{
			AssertQuery("(Person((((Age > 30) and (Age < 34)) or ((Age > 10) and (Age < 22))) or (Age == 25)))",
				delegate
				{
					var notjb = from Person p in Db()
								where (((p.Age > 30 && p.Age < 34) || (p.Age > 10 && p.Age < 22)) || p.Age == 25)
								select p;

					AssertSet(new[]
						{
							new Person { Name = "ana", Age = 20 },
							new Person { Name = "reg", Age = 25 },
							new Person { Name = "ro", Age = 32 },
						}, notjb);
				});
		}
	}
}
