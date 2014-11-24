/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.mapping;

/**
 * A mapping from db4o file source IDs/addresses to target IDs/addresses, used for defragmenting.
 * 
 * @exclude
 */
public interface IDMapping {
	/**
	 * @return a mapping for the given id. if it does refer to a system handler or the empty reference (0), returns the given id.
	 * @throws MappingNotFoundException if the given id does not refer to a system handler or the empty reference (0) and if no mapping is found
	 */
	int strictMappedID(int oldID) throws MappingNotFoundException;
	void mapIDs(int oldID,int newID, boolean isClassID);
}
