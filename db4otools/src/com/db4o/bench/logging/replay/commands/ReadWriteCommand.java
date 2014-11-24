/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.bench.logging.replay.commands;

public class ReadWriteCommand {
	
	protected final long _pos;
	protected final int _length;
	
	public ReadWriteCommand(long pos, int length) {
		_pos = pos;
		_length = length;
	}

	protected byte[] prepareBuffer() {
		return new byte[_length];
	}

}
