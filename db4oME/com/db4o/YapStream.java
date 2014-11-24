/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.ext.*;


/**
 * @exclude
 * @partial
 */
public abstract class YapStream extends YapStreamBase implements ExtObjectContainer {
	YapStream(YapStream a_parent) {
		super(a_parent);
	}
}