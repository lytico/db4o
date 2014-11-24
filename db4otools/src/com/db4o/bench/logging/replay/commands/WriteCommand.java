/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.bench.logging.replay.commands;

import com.db4o.io.*;

public class WriteCommand extends ReadWriteCommand implements IoCommand{
	
	public WriteCommand(long pos, int length) {
		super(pos, length);
	}
	
	public void replay(Bin bin){
		bin.write(_pos, prepareBuffer(), _length);
	}

}
