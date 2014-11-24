/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */

package com.db4o.filestats;

import static com.db4o.filestats.FileUsageStatsUtil.*;

/**
 * Statistics for the byte usage for a single class (instances, indices, etc.) in a db4o database file.
 * 
 * @exclude
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ClassUsageStats {
	private final String _className;
	private final long _slotUsage;
	private final long _classIndexUsage;
	private final long _fieldIndexUsage;
	private final long _miscUsage;	
	private final int _numInstances;
	
	ClassUsageStats(String className, long slotSpace, long classIndexUsage, long fieldIndexUsage, long miscUsage, int numInstances) {
		_className = className;
		_slotUsage = slotSpace;
		_classIndexUsage = classIndexUsage;
		_fieldIndexUsage = fieldIndexUsage;
		_miscUsage = miscUsage;
		_numInstances = numInstances;
	}
	
	/**
	 * @return the name of the persistent class
	 */
	public String className() {
		return _className;
	}

	/**
	 * @return number of bytes used slots containing the actual class instances
	 */
	public long slotUsage() {
		return _slotUsage;
	}

	/**
	 * @return number of bytes used for the index of class instances
	 */
	public long classIndexUsage() {
		return _classIndexUsage;
	}

	/**
	 * @return number of bytes used for field indexes, if any
	 */
	public long fieldIndexUsage() {
		return _fieldIndexUsage;
	}

	/**
	 * @return number of bytes used for features that are specific to this class (ex.: the BTree encapsulated within a {@link com.db4o.internal.collections.BigSet} instance)
	 */
	public long miscUsage() {
		return _miscUsage;
	}

	/**
	 * @return number of persistent instances of this class
	 */
	public int numInstances() {
		return _numInstances;
	}

	/**
	 * @return aggregated byte usage for this persistent class
	 */
	public long totalUsage() {
		return _slotUsage + _classIndexUsage + _fieldIndexUsage + _miscUsage;
	}

	@Override
	public String toString() {
		StringBuffer str = new StringBuffer();
		toString(str);
		return str.toString();
	}
	
	void toString(StringBuffer str) {
		str.append(className()).append("\n");
		str.append(formatLine("Slots", slotUsage()));
		str.append(formatLine("Class index", classIndexUsage()));
		str.append(formatLine("Field indices", fieldIndexUsage()));
		if(miscUsage() > 0) {
			str.append(formatLine("Misc", miscUsage()));
		}
		str.append(formatLine("Total", totalUsage()));
	}
}