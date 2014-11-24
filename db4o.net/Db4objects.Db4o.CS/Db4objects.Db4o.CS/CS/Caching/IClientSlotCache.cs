/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Caching
{
	public interface IClientSlotCache
	{
		void Add(Transaction transaction, int id, ByteArrayBuffer slot);

		ByteArrayBuffer Get(Transaction transaction, int id);
	}
}
