package com.db4o.objectmanager;

import java.util.List;
import java.util.ArrayList;

/**
 * User: treeder
 * Date: Mar 14, 2007
 * Time: 8:37:18 PM
 */
public class DumpObject {
	private List<Object> objects = new ArrayList<Object>();

	public void add(Object ob) {
		objects.add(ob);
	}

	public int size() {
		return objects.size();
	}

	public List getObjects() {
		return objects;
	}
}
