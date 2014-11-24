/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Linq;

using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests
{
	public class ParameterizedWhereTestCase : AbstractDb4oLinqTestCase
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
			Store(new Person { Name = "jb", Age = 7 });
			Store(new Person { Name = "jb", Age = 28 });
			Store(new Person { Name = "jb", Age = 34 });
		}

		public void TestEqualsLocalVariable()
		{
			var name = "jb";

			AssertQuery("(Person(Name == 'jb'))",
				delegate
				{
					var jbs = from Person p in Db()
							  where p.Name == name
							  select p;

					AssertSet(new[]
						{
							new Person { Name = "jb", Age = 24 },
							new Person { Name = "jb", Age = 7 },
							new Person { Name = "jb", Age = 28 },
							new Person { Name = "jb", Age = 34 },
						}, jbs);
				});
		}

		private string _name = "reg";

		public void TestEqualsInstanceField()
		{
			AssertQuery("(Person(Name == 'reg'))",
				delegate
				{
					var jbs = from Person p in Db()
							  where p.Name == _name
							  select p;

					AssertSet(new[]
						{
							new Person { Name = "reg", Age = 25 },
						}, jbs);
				});
		}

		private static string _static_name = "ro";

		public void TestEqualsStaticField()
		{
			AssertQuery("(Person(Name == 'ro'))",
				delegate
				{
					var jbs = from Person p in Db()
							  where p.Name == _static_name
							  select p;

					AssertSet(new[]
						{
							new Person { Name = "ro", Age = 32 },
						}, jbs);
				});
		}
	}
}
