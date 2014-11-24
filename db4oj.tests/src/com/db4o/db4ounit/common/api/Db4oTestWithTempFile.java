/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.common.api;

import com.db4o.*;
import com.db4o.config.*;

import db4ounit.extensions.*;

public class Db4oTestWithTempFile extends TestWithTempFile implements Db4oTestCase {	

	protected EmbeddedConfiguration newConfiguration() {
		return Db4oEmbedded.newConfiguration();
	}
	
}
