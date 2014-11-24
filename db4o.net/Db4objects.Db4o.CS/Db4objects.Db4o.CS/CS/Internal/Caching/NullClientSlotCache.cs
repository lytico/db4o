/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Caching;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Caching
{
	public class NullClientSlotCache : IClientSlotCache
	{
		public virtual void Add(Transaction transaction, int id, ByteArrayBuffer slot)
		{
		}

		// do nothing
		public virtual ByteArrayBuffer Get(Transaction transaction, int id)
		{
			return null;
		}
	}
}
