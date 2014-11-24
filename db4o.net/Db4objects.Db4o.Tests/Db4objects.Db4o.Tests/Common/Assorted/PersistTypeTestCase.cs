/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class PersistTypeTestCase : AbstractDb4oTestCase
	{
		public sealed class Item
		{
			public Type type;

			public Item()
			{
			}

			public Item(Type type_)
			{
				type = type_;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new PersistTypeTestCase.Item(typeof(string)));
		}

		public virtual void Test()
		{
			Assert.AreEqual(typeof(string), ((PersistTypeTestCase.Item)((PersistTypeTestCase.Item
				)RetrieveOnlyInstance(typeof(PersistTypeTestCase.Item)))).type);
		}
	}
}
#endif // !SILVERLIGHT
