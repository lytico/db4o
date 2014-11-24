/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.bench.logging.replay.commands;

import com.db4o.io.*;

public class ReadCommand extends ReadWriteCommand implements IoCommand{
	
	public ReadCommand(long pos, int length) {
		super(pos, length);
	}
	
	public void replay(Bin bin){
		bin.read(_pos, prepareBuffer(), _length);
	}

}
