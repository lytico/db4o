package com.db4o.cs.internal.objectexchange;


public class ObjectExchangeConfiguration {
	public int prefetchDepth;
	public int prefetchCount;

	public ObjectExchangeConfiguration(int prefetchDepth, int prefetchCount) {
		this.prefetchDepth = prefetchDepth;
		this.prefetchCount = prefetchCount;
	}
}