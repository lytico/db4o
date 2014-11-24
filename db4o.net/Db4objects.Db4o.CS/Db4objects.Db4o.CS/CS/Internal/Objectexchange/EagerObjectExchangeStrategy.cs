/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Caching;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public class EagerObjectExchangeStrategy : IObjectExchangeStrategy
	{
		private ObjectExchangeConfiguration _config;

		public EagerObjectExchangeStrategy(ObjectExchangeConfiguration config)
		{
			_config = config;
		}

		public virtual ByteArrayBuffer Marshall(LocalTransaction transaction, IIntIterator4
			 ids, int maxCount)
		{
			return new EagerObjectWriter(_config, transaction).Write(ids, maxCount);
		}

		public virtual IFixedSizeIntIterator4 Unmarshall(ClientTransaction transaction, IClientSlotCache
			 slotCache, ByteArrayBuffer reader)
		{
			return new CacheContributingObjectReader(transaction, slotCache, reader).Iterator
				();
		}
	}
}
