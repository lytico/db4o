/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class InvalidFieldNameConstraintTestCase : AbstractDb4oTestCase
	{
		public class Person
		{
			public string _firstName;

			public string _lastName;

			public Person(string firstName, string lastName)
			{
				_firstName = firstName;
				_lastName = lastName;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.BlockSize(8);
			config.ObjectClass(typeof(InvalidFieldNameConstraintTestCase.Person)).ObjectField
				("_firstName").Indexed(true);
			config.ObjectClass(typeof(InvalidFieldNameConstraintTestCase.Person)).ObjectField
				("_lastName").Indexed(true);
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new InvalidFieldNameConstraintTestCase.Person("John", "Doe"));
		}

		public virtual void TestQuery()
		{
			IQuery query = NewQuery(typeof(InvalidFieldNameConstraintTestCase.Person));
			query.Descend("_nonExistent").Constrain("X");
			IObjectSet result = query.Execute();
			Assert.AreEqual(0, result.Count);
		}
	}
}
