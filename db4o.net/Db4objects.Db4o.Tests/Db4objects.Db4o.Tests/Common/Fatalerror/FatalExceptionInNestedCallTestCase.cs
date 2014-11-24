/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Fatalerror;

namespace Db4objects.Db4o.Tests.Common.Fatalerror
{
	public class FatalExceptionInNestedCallTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] arguments)
		{
			new FatalExceptionInNestedCallTestCase().RunSolo();
		}

		public class Item
		{
			public FatalExceptionInNestedCallTestCase.Item _child;

			public int _depth;

			public Item()
			{
			}

			public Item(FatalExceptionInNestedCallTestCase.Item child, int depth)
			{
				_child = child;
				_depth = depth;
			}
		}

		[System.Serializable]
		public class FatalError : Exception
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			FatalExceptionInNestedCallTestCase.Item childItem = new FatalExceptionInNestedCallTestCase.Item
				(null, 1);
			FatalExceptionInNestedCallTestCase.Item parentItem = new FatalExceptionInNestedCallTestCase.Item
				(childItem, 0);
			Store(parentItem);
		}

		public virtual void Test()
		{
		}
		//	private EventRegistry eventRegistry() {
		//		return EventRegistryFactory.forObjectContainer(db());
		//	}
	}
}
