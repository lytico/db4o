/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class InterfaceFieldConstraintTestCase : AbstractDb4oTestCase
	{
		private const int Id = 42;

		public interface IIFoo
		{
		}

		public class Foo : InterfaceFieldConstraintTestCase.IIFoo
		{
			public int _id;

			public Foo(int id)
			{
				_id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new InterfaceFieldConstraintTestCase.Foo(Id));
		}

		public virtual void TestInterfaceFieldQuery()
		{
			IQuery query = NewQuery(typeof(InterfaceFieldConstraintTestCase.IIFoo));
			query.Descend("_id").Constrain(Id);
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
