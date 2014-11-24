/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TP;

namespace Db4objects.Db4o.Tests.Common.TP
{
	public class DeactivateDeletedObjectOnRollbackStrategyTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			config.Add(new TransparentPersistenceSupport(new _IRollbackStrategy_21()));
		}

		private sealed class _IRollbackStrategy_21 : IRollbackStrategy
		{
			public _IRollbackStrategy_21()
			{
			}

			public void Rollback(IObjectContainer container, object obj)
			{
				container.Ext().Deactivate(obj);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new Item("foo.tbd"));
		}

		public virtual void Test()
		{
			Item tbd = InsertAndRetrieve();
			tbd.SetName("foo.deleted");
			Db().Delete(tbd);
			Db().Rollback();
			Assert.AreEqual("foo.tbd", tbd.GetName());
		}

		private Item InsertAndRetrieve()
		{
			IQuery query = NewQuery(typeof(Item));
			query.Descend("name").Constrain("foo.tbd");
			IObjectSet set = query.Execute();
			Assert.AreEqual(1, set.Count);
			return (Item)set.Next();
		}

		public static void Main(string[] args)
		{
			new DeactivateDeletedObjectOnRollbackStrategyTestCase().RunAll();
		}
	}
}
