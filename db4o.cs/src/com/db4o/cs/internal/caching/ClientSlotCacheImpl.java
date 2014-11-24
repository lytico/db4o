/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.caching;

import com.db4o.cs.caching.*;
import com.db4o.cs.internal.*;
import com.db4o.events.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.caching.*;

public class ClientSlotCacheImpl implements ClientSlotCache {
	
	private static final Function4<Integer, ByteArrayBuffer> nullProducer = new Function4<Integer, ByteArrayBuffer>() {
		public ByteArrayBuffer apply(Integer arg) {
			return null;
		}
	};

	private final TransactionLocal<PurgeableCache4<Integer, ByteArrayBuffer>> _cache = new TransactionLocal<PurgeableCache4<Integer, ByteArrayBuffer>>() {
		public PurgeableCache4<Integer, ByteArrayBuffer> initialValueFor(Transaction transaction) {
			Config4Impl config = transaction.container().config();
			return CacheFactory.<ByteArrayBuffer>newLRUIntCache(config.prefetchSlotCacheSize());
		};
	};
	
	public ClientSlotCacheImpl(ClientObjectContainer clientObjectContainer) {
		final EventRegistry eventRegistry = EventRegistryFactory.forObjectContainer(clientObjectContainer);
		eventRegistry.activated().addListener(new EventListener4<ObjectInfoEventArgs>() {
			public void onEvent(Event4 e, ObjectInfoEventArgs args) {
				purge((Transaction) args.transaction(), (int)args.info().getInternalID());
            }
		});
	}

	public void add(Transaction provider, int id, final ByteArrayBuffer slot) {
		purge(provider, id);
		cacheOn(provider).produce(id, new Function4<Integer, ByteArrayBuffer>(){
			public ByteArrayBuffer apply(Integer arg) {
				return slot;
			}
		}, null);
    }

	public ByteArrayBuffer get(Transaction provider, int id) {
		final ByteArrayBuffer buffer = cacheOn(provider).produce(id, nullProducer, null);
		if (null == buffer) {
			return null;
		}
		buffer.seek(0);
		return buffer;
    }
	
	private void purge(Transaction provider, int id) {
		cacheOn(provider).purge(id);
	}
	
	private PurgeableCache4<Integer, ByteArrayBuffer> cacheOn(Transaction provider) {
		return provider.get(_cache).value;
	}
}
