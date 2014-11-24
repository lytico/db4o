/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class QueryingDoesNotProduceClassMetadataTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
		}

		public virtual void Test()
		{
			Db().Query(typeof(QueryingDoesNotProduceClassMetadataTestCase.Item));
			Assert.IsNull(Container().ClassMetadataForName(typeof(QueryingDoesNotProduceClassMetadataTestCase.Item
				).FullName));
		}
	}
}
