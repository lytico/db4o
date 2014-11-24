/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Caching;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public interface IObjectExchangeStrategy
	{
		ByteArrayBuffer Marshall(LocalTransaction transaction, IIntIterator4 ids, int maxCount
			);

		IFixedSizeIntIterator4 Unmarshall(ClientTransaction transaction, IClientSlotCache
			 slotCache, ByteArrayBuffer reader);
	}
}
