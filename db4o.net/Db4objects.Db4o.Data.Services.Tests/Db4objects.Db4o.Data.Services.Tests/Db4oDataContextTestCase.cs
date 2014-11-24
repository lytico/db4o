/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

#if !CF && !MONO

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services;
using System.Linq;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Linq.Tests;
using Db4oUnit;

namespace Db4objects.Db4o.Data.Services.Tests
{
	public class Db4oDataContextTestCase : AbstractDb4oLinqTestCase
	{
		public class Person
		{
			public string Name { get; set; }
			public string Title { get; set; }
			public Team Team { get; set; }
		}

		public class Team
		{
			public string Name { get; set; }
			public List<Person> People { get; private set; }

			public Team()
			{
				People = new List<Person>();
			}

			public Person AddNewMember(string name, string title)
			{
				var person = new Person { Name = name, Title = title, Team = this };
				People.Add(person);
				return person;
			}
		}

		public class ManagementDataContext : Db4oDataContext
		{
			private IObjectContainer _session;

			public ManagementDataContext(IObjectContainer session)
			{
				_session = session;
			}

			protected override IObjectContainer OpenSession()
			{
				return _session;
			}

			public IQueryable<Person> Hackers
			{
				get { return Container.AsQueryable<Person>(); }
			}

			public IQueryable<Team> Teams
			{
				get { return Container.AsQueryable<Team>(); }
			}
		}

		protected override void Store()
		{
			var hatch = new Team { Name = "Hatch People" };
			hatch.AddNewMember("John Locke", "Half-God");
			hatch.AddNewMember("Mr Eko", "Believer");
			hatch.AddNewMember("Desmond", "Runner");

			var beach = new Team { Name = "Beach People" };
			beach.AddNewMember("Jack", "Doctor");
			beach.AddNewMember("Kate", "Cuty");
			beach.AddNewMember("Sawyer", "The Other dude");

			var other = new Team { Name = "The Others" };
			other.AddNewMember("Ben", "Mastermind");
			other.AddNewMember("Juliet", "Wannabe Cuty");

			Store(hatch);
			Store(beach);
			Store(other);
		}

		private ManagementDataContext CreateContext()
		{
			return new ManagementDataContext((IEmbeddedObjectContainer) Db());
		}

		private void WithContext(Action<ManagementDataContext> action)
		{
			var context = CreateContext();
			action(context);
			context.SaveChanges();
		}

		public void TestGetValue()
		{
			var team = new Team { Name = "Foo" };
			var person = new Person { Name = "Mr Foo", Team = team };

			WithContext(context =>
			{
				Assert.AreEqual("Mr Foo", context.GetValue(person, "Name"));
				Assert.AreEqual(team, context.GetValue(person, "Team"));

				Assert.Expect(typeof(DataServiceException), () => context.GetValue(person, "Fubar"));
			});
		}

		public void TestSetReference()
		{
			var fooTeam = new Team { Name = "foo" };
			var barTeam = new Team { Name = "bar" };

			var person = new Person { Name = "Mr Foo" };
			person.Team = fooTeam;

			WithContext(context =>
			{
				context.SetReference(person, "Team", barTeam);
				Assert.AreEqual(barTeam, person.Team);

				Assert.Expect(typeof(DataServiceException), () => context.SetReference(person, "Fubar", "..."));
			});
		}

		public void TestSetValue()
		{
			var team = new Team { Name = "db4o" };

			WithContext(context =>
			{
				context.SetValue(team, "Name", "db4occ3");
				Assert.AreEqual("db4occ3", team.Name);

				Assert.Expect(typeof(DataServiceException), () => context.SetValue(team, "Fubar", "..."));
			});
		}

		public void TestResolveResource()
		{
			var team = new Team { Name = "db4o" };

			WithContext(context => Assert.AreEqual(team, context.ResolveResource(team)));
		}

		public void TestCreateResource()
		{
			WithContext(context =>
			{
				var team = context.CreateResource("Teams", typeof(Team).FullName) as Team;

				Assert.IsNotNull(team);
				Assert.IsInstanceOf(typeof(Team), team);

				Assert.Expect(typeof(DataServiceException), () => context.CreateResource("Teams", "Fubar"));
			});
		}

		public void _TestResetResource()
		{
			var team = new Team { Name = "db4o" };

			WithContext(context =>
			{
				var reseted = context.ResetResource(team) as Team;

				Assert.IsNotNull(reseted);
				Assert.IsNull(reseted.Name);
				Assert.AreEqual(0, reseted.People.Count);
			});
		}

		private Person GetJohnLocke()
		{
			return (from Person p in Db() where p.Name == "John Locke" select p).FirstOrDefault();
		}

		public void TestSaveChanges()
		{
			var jlocke = GetJohnLocke();

			Assert.AreEqual("Half-God", jlocke.Title);

			WithContext(context => context.SetValue(jlocke, "Title", "Miracle Man"));

			Reopen();

			jlocke = GetJohnLocke();

			Assert.AreEqual("Miracle Man", jlocke.Title);
		}

		public void TestClearChanges()
		{
			var jlocke = GetJohnLocke();

			Assert.AreEqual("Half-God", jlocke.Title);

			WithContext(context =>
			{
				context.SetValue(jlocke, "Title", "Miracle Man");
				Assert.AreEqual("Miracle Man", jlocke.Title);
				context.ClearChanges();
			});

			Reopen();

			jlocke = GetJohnLocke();

			Assert.AreEqual("Half-God", jlocke.Title);
		}

		public void TestDeleteResource()
		{
			var jlocke = GetJohnLocke();

			Assert.IsNotNull(jlocke);

			WithContext(context =>
			{
				context.DeleteResource(jlocke);
			});

			Assert.IsNull(GetJohnLocke());
		}

		public void TestGetResource()
		{
			WithContext(context =>
			{
				var query = context.Hackers.Where(p => p.Name == "John Locke");

				var jlocke = context.GetResource(query, typeof(Person).FullName) as Person;

				Assert.IsNotNull(jlocke);
				Assert.AreEqual("John Locke", jlocke.Name);
			});
		}

		public void TestAddToIListCollection()
		{
			var team = (from Team t in Db() where t.Name == "Beach People" select t).First();
			var hurley = new Person { Name = "Hurley", Title = "Dude", Team = team };

			WithContext(context =>
			{
				var count = team.People.Count;

				context.AddReferenceToCollection(team, "People", hurley);

				Assert.AreEqual(count + 1, team.People.Count);
				Assert.IsTrue((from p in team.People where p.Name == "Hurley" select p).Any());
			});
		}

		public void TestRemoveToIListCollection()
		{
			var team = (from Team t in Db() where t.Name == "Beach People" select t).First();
			var kate = (from p in team.People where p.Name == "Kate" select p).First();

			WithContext(context =>
			{
				var count = team.People.Count;

				context.RemoveReferenceFromCollection(team, "People", kate);

				Assert.AreEqual(count - 1, team.People.Count);
				Assert.IsFalse((from p in team.People where p.Name == "Kate" select p).Any());
			});
		}

		public class People : ICollection<Person>
		{
			List<Person> _list = new List<Person>();

			public void Add(Person item)
			{
				_list.Add(item);
			}

			public void Clear()
			{
				_list.Clear();
			}

			public bool Contains(Person item)
			{
				return _list.Contains(item);
			}

			public void CopyTo(Person[] array, int arrayIndex)
			{
				_list.CopyTo(array, arrayIndex);
			}

			public int Count
			{
				get { return _list.Count; }
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public bool Remove(Person item)
			{
				return _list.Remove(item);
			}

			public IEnumerator<Person> GetEnumerator()
			{
				return _list.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		class Hatch
		{
			public string Name { get; set; }
			public People People { get; private set; }

			public Hatch()
			{
				People = new People();
			}
		}

		static Hatch CreateHatch()
		{
			var hatch = new Hatch { Name = "La Perle" };
			hatch.People.Add(new Person { Name = "Desmond" });
			hatch.People.Add(new Person { Name = "Charlie" });
			return hatch;
		}

		public void TestAddToCollectionWithReflection()
		{
			var hatch = CreateHatch();
			var eko = new Person { Name = "Mr Eko", Title = "Warlord" };

			WithContext(context =>
			{
				var count = hatch.People.Count;

				context.AddReferenceToCollection(hatch, "People", eko);

				Assert.AreEqual(count + 1, hatch.People.Count);
				Assert.IsTrue((from p in hatch.People where p.Name == "Mr Eko" select p).Any());
			});
		}

		public void TestRemoveFromCollectionWithReflection()
		{
			var hatch = CreateHatch();
			var charlie = (from p in hatch.People where p.Name == "Charlie" select p).First();

			WithContext(context =>
			{
				var count = hatch.People.Count;

				context.RemoveReferenceFromCollection(hatch, "People", charlie);

				Assert.AreEqual(count - 1, hatch.People.Count);
				Assert.IsFalse((from p in hatch.People where p.Name == "Charlie" select p).Any());
			});
		}

		public void TestInvalidCollectionManipulation()
		{
			var eko = new Person { Name = "Mr Eko", Title = "Warlord" };

			WithContext(context =>
			{
				Assert.Expect(typeof(DataServiceException), () => context.AddReferenceToCollection(eko, "Name", eko));
				Assert.Expect(typeof(DataServiceException), () => context.RemoveReferenceFromCollection(eko, "Name", eko));
			});
		}
	}
}

#endif
