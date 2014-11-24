/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.io;


/**
 * @exclude
 */
public class BinConfiguration {
	
	private final String _uri;
	
	private final boolean _lockFile;
	
	private final long _initialLength;
	
	private final boolean _readOnly;
	
	private final int _blockSize;

	public BinConfiguration(String uri, boolean lockFile, long initialLength, boolean readOnly) {
		this(uri, lockFile, initialLength, readOnly, 1);
	}

	public BinConfiguration(String uri, boolean lockFile, long initialLength, boolean readOnly, int blockSize) {
		_uri = uri;
		_lockFile = lockFile;
		_initialLength = initialLength;
		_readOnly = readOnly;
		_blockSize = blockSize;
	}
	
	public String uri(){
		return _uri;
	}
	
	public boolean lockFile(){
		return _lockFile;
	}
	
	public long initialLength(){
		return _initialLength;
	}
	
	public boolean readOnly(){
		return _readOnly;
	}
	
	public int blockSize() {
		return _blockSize;
	}
	
	@Override
	public String toString() {
		return "BinConfiguration(Uri: " + _uri + ", Locked: " + _lockFile + ", ReadOnly: " + _readOnly + ", BlockSize: " + _blockSize + ")";
	}
}
