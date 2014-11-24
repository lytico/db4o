/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.config;

import com.db4o.ext.*;

/**
 * db4o-specific exception.<br><br>
 * This exception is thrown when a global configuration 
 * setting is attempted on an open object container.
 *@see com.db4o.config.Configuration#blockSize(int)
 *@see com.db4o.config.Configuration#encrypt(boolean)
 *@see com.db4o.config.Configuration#password(String)
 */
public class GlobalOnlyConfigException extends Db4oRecoverableException {
	
}
