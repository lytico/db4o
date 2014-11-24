/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.defragment;

import com.db4o.foundation.io.*;

public abstract class SlotDefragmentTestConstants {
	
	private final static String FILENAME = Path4.getTempFileName();
	private final static String BACKUPFILENAME = FILENAME+".backup";

	private SlotDefragmentTestConstants() {
	}
}
