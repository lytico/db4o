/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class SimplestPossibleNullMemberTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public SimplestPossibleNullMemberTestCase.Item _item;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new SimplestPossibleNullMemberTestCase.Item());
		}

		public virtual void Test()
		{
			SimplestPossibleNullMemberTestCase.Item item = (SimplestPossibleNullMemberTestCase.Item
				)((SimplestPossibleNullMemberTestCase.Item)RetrieveOnlyInstance(typeof(SimplestPossibleNullMemberTestCase.Item
				)));
			Assert.IsNull(item._item);
		}
	}
}
