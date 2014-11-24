/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Mocking;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TP;

namespace Db4objects.Db4o.Tests.Common.TP
{
	public class RollbackStrategyTestCase : AbstractDb4oTestCase
	{
		private readonly RollbackStrategyMock _mock = new RollbackStrategyMock();

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentPersistenceSupport(_mock));
		}

		public virtual void TestRollbackStrategyIsCalledForChangedObjects()
		{
			Item item1 = StoreItem("foo");
			Item item2 = StoreItem("bar");
			StoreItem("baz");
			Change(item1);
			Change(item2);
			_mock.Verify(new MethodCall[0]);
			Db().Rollback();
			_mock.VerifyUnordered(new MethodCall[] { new MethodCall("rollback", new object[] 
				{ Db(), item1 }), new MethodCall("rollback", new object[] { Db(), item2 }) });
		}

		private void Change(Item item)
		{
			item.SetName(item.GetName() + "*");
		}

		private Item StoreItem(string name)
		{
			Item item = new Item(name);
			Store(item);
			return item;
		}

		public static void Main(string[] args)
		{
			new RollbackStrategyTestCase().RunAll();
		}
	}
}
