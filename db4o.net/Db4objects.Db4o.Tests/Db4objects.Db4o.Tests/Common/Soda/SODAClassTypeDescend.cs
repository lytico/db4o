/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class SODAClassTypeDescend : AbstractDb4oTestCase
	{
		public class DataA
		{
			public SODAClassTypeDescend.DataB _val;
			// COR-471
		}

		public class DataB
		{
			public SODAClassTypeDescend.DataA _val;
		}

		public class DataC
		{
			public SODAClassTypeDescend.DataC _next;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			SODAClassTypeDescend.DataA objectA = new SODAClassTypeDescend.DataA();
			SODAClassTypeDescend.DataB objectB = new SODAClassTypeDescend.DataB();
			objectA._val = objectB;
			objectB._val = objectA;
			Store(objectB);
			// just to show that the descend to "_val" actually is
			// recognized - this one doesn't show up in the result
			Store(new SODAClassTypeDescend.DataC());
		}

		public virtual void TestFieldConstrainedToType()
		{
			IQuery query = NewQuery();
			query.Descend("_val").Constrain(typeof(SODAClassTypeDescend.DataA));
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			Assert.IsInstanceOf(typeof(SODAClassTypeDescend.DataB), result.Next());
		}
	}
}
