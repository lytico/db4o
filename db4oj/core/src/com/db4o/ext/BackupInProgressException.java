/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.ext;

/**
 * db4o-specific exception. <br><br>
 * This exception is thrown when the current 
 * {@link com.db4o.ext.ExtObjectContainer#backup(String) backup }
 * process encounters another backup process already running.
 */
public class BackupInProgressException extends Db4oRecoverableException {

}
