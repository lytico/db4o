/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	/// <exclude></exclude>
	public class ListTypeHandlerCascadedDeleteTestCase : AbstractDb4oTestCase
	{
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			new ListTypeHandlerCascadedDeleteTestCase().RunSolo();
		}

		public class Item
		{
			public object _untypedList;

			public ArrayList _typedList;
		}

		public class Element
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(ListTypeHandlerCascadedDeleteTestCase.Item)).CascadeOnDelete
				(true);
			config.ObjectClass(typeof(ArrayList)).CascadeOnDelete(true);
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(ArrayList))
				, new CollectionTypeHandler());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			ListTypeHandlerCascadedDeleteTestCase.Item item = new ListTypeHandlerCascadedDeleteTestCase.Item
				();
			item._untypedList = new ArrayList();
			((IList)item._untypedList).Add(new ListTypeHandlerCascadedDeleteTestCase.Element(
				));
			item._typedList = new ArrayList();
			item._typedList.Add(new ListTypeHandlerCascadedDeleteTestCase.Element());
			Store(item);
		}

		public virtual void TestCascadedDelete()
		{
			ListTypeHandlerCascadedDeleteTestCase.Item item = (ListTypeHandlerCascadedDeleteTestCase.Item
				)RetrieveOnlyInstance(typeof(ListTypeHandlerCascadedDeleteTestCase.Item));
			Db4oAssert.PersistedCount(2, typeof(ListTypeHandlerCascadedDeleteTestCase.Element
				));
			Db().Delete(item);
			Db().Purge();
			Db().Commit();
			Db4oAssert.PersistedCount(0, typeof(ListTypeHandlerCascadedDeleteTestCase.Item));
			Db4oAssert.PersistedCount(0, typeof(ArrayList));
			Db4oAssert.PersistedCount(0, typeof(ListTypeHandlerCascadedDeleteTestCase.Element
				));
		}

		public virtual void TestArrayListCount()
		{
			Db4oAssert.PersistedCount(2, typeof(ArrayList));
		}
	}
}
