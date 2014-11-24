package com.db4o.db4ounit.common.ta.mixed;

import com.db4o.db4ounit.common.ta.*;

public class MixedArrayItem {
    
	public Object[] objects;

	public MixedArrayItem() {

	}

	public MixedArrayItem(int v) {
		objects = new Object[4];
		objects[0] = LinkedList.newList(v);
		objects[1] = new TItem(v);
		objects[2] = LinkedList.newList(v);
		objects[3] = new TItem(v);
	}
}
