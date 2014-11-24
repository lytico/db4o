package com.db4o.objectManager.v2.tree;

import java.util.Map;

/**
 * User: treeder
 * Date: Nov 11, 2006
 * Time: 5:17:29 PM
 */
public class MapEntry extends Object {
	private Map.Entry entry;

	public MapEntry(Map.Entry entry) {
		this.entry = entry;
	}

	public String toString() {
		return "Map Entry: " + entry.getKey();
	}

	public Map.Entry getEntry() {
		return entry;
	}
}
