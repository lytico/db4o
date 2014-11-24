/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.types;

/**
 * marks objects as second class objects.
 * 
 * <br><br>Currently this interface is for internal use only
 * to help discard com.db4o.config.Entry objects in the
 * Defragment process.
 * <br><br>For future versions this interface is planned to 
 * mark objects that:<br>
 * - are not to be held in the reference mechanism<br>
 * - should always be activated with their parent objects<br>
 * - should always be deleted with their parent objects<br>
 * - should always be deleted if they are not referenced any
 * longer.<br>
 */
public interface SecondClass extends Db4oType{
}
