/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	/// <exclude></exclude>
	public class OrderByParentFieldTestCase : AbstractDb4oTestCase
	{
		public class Parent
		{
			public string _name;

			public Parent(string name)
			{
				_name = name;
			}
		}

		public class Child : OrderByParentFieldTestCase.Parent
		{
			public int _age;

			public Child(string name, int age) : base(name)
			{
				_age = age;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new OrderByParentFieldTestCase.Child("One", 1));
			Store(new OrderByParentFieldTestCase.Child("Two", 2));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IQuery query = NewQuery(typeof(OrderByParentFieldTestCase.Child));
			query.Descend("_name").OrderAscending();
			query.Execute();
		}
	}
}
