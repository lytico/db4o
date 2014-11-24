/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	/// <exclude></exclude>
	public class MultiFieldIndexQueryTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new MultiFieldIndexQueryTestCase().RunSolo();
		}

		public class Book
		{
			public MultiFieldIndexQueryTestCase.Person[] authors;

			public string title;

			public Book()
			{
			}

			public Book(string title, MultiFieldIndexQueryTestCase.Person[] authors)
			{
				this.title = title;
				this.authors = authors;
			}

			public override string ToString()
			{
				string ret = title;
				if (authors != null)
				{
					for (int i = 0; i < authors.Length; i++)
					{
						ret += "\n  " + authors[i].ToString();
					}
				}
				return ret;
			}
		}

		public class Person
		{
			public string firstName;

			public string lastName;

			public Person()
			{
			}

			public Person(string firstName, string lastName)
			{
				this.firstName = firstName;
				this.lastName = lastName;
			}

			public override string ToString()
			{
				return "Person " + firstName + " " + lastName;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			IndexAllFields(config, typeof(MultiFieldIndexQueryTestCase.Book));
			IndexAllFields(config, typeof(MultiFieldIndexQueryTestCase.Person));
		}

		protected virtual void IndexAllFields(IConfiguration config, Type clazz)
		{
			FieldInfo[] fields = Sharpen.Runtime.GetDeclaredFields(clazz);
			for (int i = 0; i < fields.Length; i++)
			{
				IndexField(config, clazz, fields[i].Name);
			}
			Type superclass = clazz.BaseType;
			if (superclass != null)
			{
				IndexAllFields(config, superclass);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			MultiFieldIndexQueryTestCase.Person aaron = new MultiFieldIndexQueryTestCase.Person
				("Aaron", "OneOK");
			MultiFieldIndexQueryTestCase.Person bill = new MultiFieldIndexQueryTestCase.Person
				("Bill", "TwoOK");
			MultiFieldIndexQueryTestCase.Person chris = new MultiFieldIndexQueryTestCase.Person
				("Chris", "ThreeOK");
			MultiFieldIndexQueryTestCase.Person dave = new MultiFieldIndexQueryTestCase.Person
				("Dave", "FourOK");
			MultiFieldIndexQueryTestCase.Person neil = new MultiFieldIndexQueryTestCase.Person
				("Neil", "Notwanted");
			MultiFieldIndexQueryTestCase.Person nat = new MultiFieldIndexQueryTestCase.Person
				("Nat", "Neverwanted");
			Db().Store(new MultiFieldIndexQueryTestCase.Book("Persistence possibilities", new 
				MultiFieldIndexQueryTestCase.Person[] { aaron, bill, chris }));
			Db().Store(new MultiFieldIndexQueryTestCase.Book("Persistence using S.O.D.A.", new 
				MultiFieldIndexQueryTestCase.Person[] { aaron }));
			Db().Store(new MultiFieldIndexQueryTestCase.Book("Persistence using JDO", new MultiFieldIndexQueryTestCase.Person
				[] { bill, dave }));
			Db().Store(new MultiFieldIndexQueryTestCase.Book("Don't want to find Phil", new MultiFieldIndexQueryTestCase.Person
				[] { aaron, bill, neil }));
			Db().Store(new MultiFieldIndexQueryTestCase.Book("Persistence by Jeff", new MultiFieldIndexQueryTestCase.Person
				[] { nat }));
		}

		public virtual void Test()
		{
			IQuery qBooks = NewQuery();
			qBooks.Constrain(typeof(MultiFieldIndexQueryTestCase.Book));
			qBooks.Descend("title").Constrain("Persistence").Like();
			IQuery qAuthors = qBooks.Descend("authors");
			IQuery qFirstName = qAuthors.Descend("firstName");
			IQuery qLastName = qAuthors.Descend("lastName");
			IConstraint cAaron = qFirstName.Constrain("Aaron").And(qLastName.Constrain("OneOK"
				));
			IConstraint cBill = qFirstName.Constrain("Bill").And(qLastName.Constrain("TwoOK")
				);
			cAaron.Or(cBill);
			IObjectSet results = qAuthors.Execute();
			Assert.AreEqual(4, results.Count);
			while (results.HasNext())
			{
				MultiFieldIndexQueryTestCase.Person person = (MultiFieldIndexQueryTestCase.Person
					)results.Next();
				Assert.IsTrue(person.lastName.EndsWith("OK"));
			}
		}
	}
}
