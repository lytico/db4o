/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.io;

import java.io.*;
import java.util.*;

import com.db4o.ext.*;

/**
 * {@link Storage} implementation that produces {@link Bin} instances
 * that operate in memory.
 * Use this {@link Storage} to work with db4o as an in-memory database. 
 */
public class MemoryStorage implements Storage {

	private final Map<String, MemoryBin> _bins = new HashMap<String, MemoryBin>();
	private final GrowthStrategy _growthStrategy;

	public MemoryStorage() {
		this(new DoublingGrowthStrategy());
	}

	public MemoryStorage(GrowthStrategy growthStrategy) {
		_growthStrategy = growthStrategy;
	}
	
	/**
	 * returns true if a MemoryBin with the given URI name already exists
	 * in this Storage.
	 */
	public boolean exists(String uri) {
		return _bins.containsKey(uri);
	}

	/**
	 * opens a MemoryBin for the given URI (name can be freely chosen).
	 */
	public Bin open(BinConfiguration config) throws Db4oIOException {
		final Bin storage = produceStorage(config);
		return config.readOnly() ? new ReadOnlyBin(storage) : storage;
	}

	/**
	 * Returns the memory bin for the given URI for external use.
	 */
	public MemoryBin bin(String uri) {
		return _bins.get(uri);
	}

	/**
	 * Registers the given bin for this storage with the given URI.
	 */
	public void bin(String uri, MemoryBin bin) {
		_bins.put(uri, bin);
	}

	private Bin produceStorage(BinConfiguration config) {
	    final Bin storage = bin(config.uri());
		if (null != storage) {
			return storage;
		}
		final MemoryBin newStorage = new MemoryBin(new byte[(int)config.initialLength()], _growthStrategy);
		_bins.put(config.uri(), newStorage);
		return newStorage;
    }

	public void delete(String uri) throws IOException {
		_bins.remove(uri);
	}

	public void rename(String oldUri, String newUri) throws IOException {
		MemoryBin bin = _bins.remove(oldUri);
		if(bin == null) {
			throw new IOException("Bin not found: " + oldUri);
		}
		_bins.put(newUri, bin);
		
	}

}
