/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.caching;

import java.util.*;

import com.db4o.foundation.*;

/**
 * @exclude
 */
public class CacheStatistics <K,V> implements Cache4<K, V>{
	
	private final Cache4<K, V> _delegate;
	
	private int _calls;
	
	private int _misses;
	
	public CacheStatistics (Cache4 delegate_){
		_delegate = delegate_;
	}

	public V produce(K key, final Function4<K, V> producer, Procedure4<V> onDiscard) {
		_calls++;
		Function4<K, V> delegateProducer = new Function4<K, V>(){
			public V apply(K arg) {
				_misses++;
				return producer.apply(arg);
			}
		};
		return _delegate.produce(key, delegateProducer, onDiscard);
	}

	public Iterator<V> iterator() {
		return _delegate.iterator();
	}
	
	public int calls() {
		return _calls;
	}
	
	public int misses() {
		return _misses;
	}
	
	public String toString(){
		return "Cache statistics  Calls:" + _calls + " Misses:" + _misses;
	}

}
