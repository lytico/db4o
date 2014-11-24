/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.defragment;

/**
 * Listener for defragmentation process messages.
 * 
 * @see Defragment
 */
public interface DefragmentListener {
	/**
	 * This method will be called when the defragment process encounters 
	 * file layout anomalies during the defragmentation process. 
	 * 
	 * @param info The message from the defragmentation process.
	 */
	void notifyDefragmentInfo(DefragmentInfo info);
}
