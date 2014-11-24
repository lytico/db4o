/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.encoding;

import com.db4o.config.encoding.*;

/**
 * @exclude
 */
public class UnicodeStringEncoding extends LatinStringEncoding {
	
	protected LatinStringIO createStringIo(StringEncoding encoding) {
		return new UnicodeStringIO();
	}

}
