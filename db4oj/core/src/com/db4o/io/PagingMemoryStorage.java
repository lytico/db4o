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
public class PagingMemoryStorage implements Storage {

	private static final int DEFAULT_PAGESIZE = 4096;
	private final Map<String, Bin> _binsByUri = new HashMap<String, Bin>();
	private final int _pageSize;

	public PagingMemoryStorage() {
		this(DEFAULT_PAGESIZE);
	}

	public PagingMemoryStorage(int pageSize) {
		_pageSize = pageSize;
	}
	
	/**
	 * returns true if a MemoryBin with the given URI name already exists
	 * in this Storage.
	 */
	public boolean exists(String uri) {
		return _binsByUri.containsKey(uri);
	}

	/**
	 * opens a MemoryBin for the given URI (name can be freely chosen).
	 */
	public Bin open(BinConfiguration config) throws Db4oIOException {
		final Bin bin = acquireBin(config);
		return config.readOnly() ? new ReadOnlyBin(bin) : bin;
	}

	/**
	 * Returns the memory bin for the given URI for external use.
	 */
	public Bin bin(String uri) {
		return _binsByUri.get(uri);
	}

	/**
	 * Registers the given bin for this storage with the given URI.
	 */
	public void bin(String uri, Bin bin) {
		_binsByUri.put(uri, bin);
	}

	private Bin acquireBin(BinConfiguration config) {
	    final Bin storage = bin(config.uri());
		if (null != storage) {
			return storage;
		}
		final Bin newStorage = produceBin(config, _pageSize);
		_binsByUri.put(config.uri(), newStorage);
		return newStorage;
    }

	protected Bin produceBin(BinConfiguration config, int pageSize) {
		return new PagingMemoryBin(pageSize, config.initialLength());
	}

	public void delete(String uri) throws IOException {
		_binsByUri.remove(uri);
	}

	public void rename(String oldUri, String newUri) throws IOException {
		Bin bin = _binsByUri.remove(oldUri);
		if (bin == null) {
			throw new IOException("Bin not found: " + oldUri);
		}
		_binsByUri.put(newUri, bin);
		
	}

}
