/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.handlers;


class Post7_1ObjectContainerAdapter extends AbstractObjectContainerAdapter {

	public void store(Object obj) {
		db.store(obj);
	}

	public void store(Object obj, int depth) {
		db.store(obj, depth);
	}

}