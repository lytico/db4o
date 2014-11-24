package com.db4o.objectmanager.api;

import java.util.*;

/**
 * User: treeder
 * Date: Aug 7, 2006
 * Time: 10:40:11 AM
 */
public final class IndexType {

	public final static IndexType FLAT=new IndexType("Flat");
	public final static IndexType BTREE=new IndexType("BTree");
	
	public final static List ALL=
			Collections.unmodifiableList(Arrays.asList(new IndexType[]{FLAT,BTREE}));

	private String _name;
	
	private IndexType(String name) {
		_name=name;
	}
	
	public String name() {
		return _name;
	}
}
