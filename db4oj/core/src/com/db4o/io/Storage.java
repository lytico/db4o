/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.io;

import java.io.*;

import com.db4o.config.*;
import com.db4o.ext.*;

/**
 * Base interface for Storage adapters that open a {@link Bin}
 * to store db4o database data to.
 * @see FileConfiguration#storage(Storage) 
 */
public interface Storage {
	
	/**
	 * opens a {@link Bin} to store db4o database data. 
	 */
	Bin open(BinConfiguration config) throws Db4oIOException;

	/**
	 * returns true if a Bin (file or memory) exists with the passed name. 
	 */
	boolean exists(String uri);

	/**
	 * Deletes the bin for the given URI from the storage.
	 * @since 7.9
	 * @param uri bin URI
	 * @throws IOException if the bin could not be deleted
	 */
	void delete(String uri) throws IOException;

	/**
	 * Renames the bin for the given old URI to the new URI. If a bin for the new URI
	 * exists, it will be overwritten.
	 * @since 7.9
	 * @param oldUri URI of the existing bin
	 * @param newUri future URI of the bin
	 * @throws IOException if the bin could not be deleted
	 */
	void rename(String oldUri, String newUri) throws IOException;
}
