/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.filestats;

import com.db4o.internal.*;

/**
* @exclude
*/
@decaf.Ignore(decaf.Platform.JDK11)
interface MiscCollector {
	long collectFor(LocalObjectContainer db, int id, SlotMap slotMap);
}