/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;


public class PlainAllocateDriver extends IdSystemDriver {

	public PlainAllocateDriver(IdSystemEngine engine) {
		super(engine);
	}
		
	public void lapAllocate() {
		int numIds = setup().getObjectCount();
		for (int idIdx = 0; idIdx < numIds; idIdx++) {
			idSystem().newId();
		}
	}

}
