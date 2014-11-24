/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Caching;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.CS.Internal.Caching
{
	public class ClientSlotCacheImpl : IClientSlotCache
	{
		private sealed class _IFunction4_14 : IFunction4
		{
			public _IFunction4_14()
			{
			}

			public object Apply(object arg)
			{
				return null;
			}
		}

		private static readonly IFunction4 nullProducer = new _IFunction4_14();

		private sealed class _TransactionLocal_20 : TransactionLocal
		{
			public _TransactionLocal_20()
			{
			}

			public override object InitialValueFor(Transaction transaction)
			{
				Config4Impl config = transaction.Container().Config();
				return CacheFactory.NewLRUIntCache(config.PrefetchSlotCacheSize());
			}
		}

		private readonly TransactionLocal _cache = new _TransactionLocal_20();

		public ClientSlotCacheImpl(ClientObjectContainer clientObjectContainer)
		{
			IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(clientObjectContainer
				);
			eventRegistry.Activated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_29(this).OnEvent);
		}

		private sealed class _IEventListener4_29
		{
			public _IEventListener4_29(ClientSlotCacheImpl _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing.Purge((Transaction)((ObjectInfoEventArgs)args).Transaction(), (int
					)((ObjectInfoEventArgs)args).Info.GetInternalID());
			}

			private readonly ClientSlotCacheImpl _enclosing;
		}

		public virtual void Add(Transaction provider, int id, ByteArrayBuffer slot)
		{
			Purge(provider, id);
			CacheOn(provider).Produce(id, new _IFunction4_38(slot), null);
		}

		private sealed class _IFunction4_38 : IFunction4
		{
			public _IFunction4_38(ByteArrayBuffer slot)
			{
				this.slot = slot;
			}

			public object Apply(object arg)
			{
				return slot;
			}

			private readonly ByteArrayBuffer slot;
		}

		public virtual ByteArrayBuffer Get(Transaction provider, int id)
		{
			ByteArrayBuffer buffer = ((ByteArrayBuffer)CacheOn(provider).Produce(id, nullProducer
				, null));
			if (null == buffer)
			{
				return null;
			}
			buffer.Seek(0);
			return buffer;
		}

		private void Purge(Transaction provider, int id)
		{
			CacheOn(provider).Purge(id);
		}

		private IPurgeableCache4 CacheOn(Transaction provider)
		{
			return ((IPurgeableCache4)provider.Get(_cache).value);
		}
	}
}
