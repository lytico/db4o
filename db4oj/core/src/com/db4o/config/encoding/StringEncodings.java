/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.config.encoding;

import com.db4o.config.*;
import com.db4o.internal.encoding.*;

/**
 * All built in String encodings
 * @see Configuration#stringEncoding(StringEncoding)
 */
public class StringEncodings {
	
	public static StringEncoding utf8() {
		return new UTF8StringEncoding();
	}
	
	public static StringEncoding unicode() {
		return new UnicodeStringEncoding();
	}
	
	public static StringEncoding latin() {
		return new LatinStringEncoding();
	}

}
