/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.bench.logging.replay.commands;

import com.db4o.io.*;

public class SyncCommand implements IoCommand{

	public void replay(Bin bin) {
		bin.sync();
	}

}
