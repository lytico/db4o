/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class ByteCoercionTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public byte _b;

			public Item(byte b)
			{
				_b = b;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ByteCoercionTestCase.Item((byte)42));
		}

		public virtual void TestByteCoercion()
		{
			IQuery query = NewQuery(typeof(ByteCoercionTestCase.Item));
			query.Descend("_b").Constrain(42);
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
