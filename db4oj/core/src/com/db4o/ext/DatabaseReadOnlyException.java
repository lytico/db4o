/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

/**
 * db4o-specific exception.<br><br>
 * This exception is thrown when a write operation is attempted 
 * on a database in a read-only mode.
 * @see com.db4o.config.Configuration#readOnly(boolean) 
 */
public class DatabaseReadOnlyException extends Db4oRecoverableException {

}
