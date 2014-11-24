/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class SortingNotAvailableField : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new SortingNotAvailableField().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			base.Store();
			Db().Store(new SortingNotAvailableField.OrderedItem());
			Db().Store(new SortingNotAvailableField.OrderedItem());
		}

		public virtual void TestOrderWithRightFieldName()
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(SortingNotAvailableField.OrderedItem));
			query.Descend("myOrder").OrderAscending();
			IObjectSet result = query.Execute();
			Assert.AreEqual(2, result.Count);
		}

		public virtual void TestOrderWithWrongFieldName()
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(SortingNotAvailableField.OrderedItem));
			query.Descend("myorder").OrderAscending();
			IObjectSet result = query.Execute();
			Assert.AreEqual(2, result.Count);
		}

		public class OrderedItem
		{
			public int myOrder = 42;
		}
	}
}
