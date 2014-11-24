/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	[System.Serializable]
	public class ItemPredicate : Predicate
	{
		public virtual bool Match(EmbeddedClientObjectContainerTestCase.Item item)
		{
			return true;
		}
	}
}
