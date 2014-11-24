/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.io;

import java.io.*;

import com.db4o.ext.*;

/**
 * Wrapper base class for all classes that wrap Storage.
 * Each class that adds functionality to a Storage can
 * extend this class.
 * @see BinDecorator 
 */
public class StorageDecorator implements Storage {

	protected final Storage _storage;

	public StorageDecorator(Storage storage) {
		_storage = storage;
	}

	public boolean exists(String uri) {
    	return _storage.exists(uri);
    }
	
	public Bin open(BinConfiguration config) throws Db4oIOException {
		return decorate(config, _storage.open(config));
	}

	protected Bin decorate(BinConfiguration config, Bin bin) {
		return bin;
    }

	public void delete(String uri) throws IOException {
		_storage.delete(uri);
	}

	public void rename(String oldUri, String newUri) throws IOException {
		_storage.rename(oldUri, newUri);
	}
}