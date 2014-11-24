/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.handlers;

import java.lang.reflect.*;

import db4ounit.*;

/** @sharpen.partial */
public class Pre7_1ObjectContainerAdapter extends AbstractObjectContainerAdapter {

	public void store(Object obj) {
		storeObject(obj);
	}

	public void store(Object obj, int depth) {
		storeObject(obj, depth);
	}

	private void storeObject(Object obj) {
		try {
			storeInternal(resolveSetMethod(), new Object[] { obj });
		} catch (Exception e) {
			Assert.fail("Call to set method failed.", e);
		}
	}

	private void storeObject(Object obj, int depth) {
		try {
			storeInternal(resolveSetWithDepthMethod(), new Object[] { obj, depth });
		} catch (Exception e) {
			Assert.fail("Call to set method failed.", e);
		}
	}

	public void storeInternal(Method method, Object[] args) {

		try {
			method.invoke(db, args);
		} catch (Exception e) {
			Assert.fail(e.toString());
			e.printStackTrace();
		}
	}

	 
	private Method resolveSetWithDepthMethod() throws Exception {
		if (setWithDepthMethod != null) return setWithDepthMethod;
		
		setWithDepthMethod = db.getClass().getMethod(setMethodName(), new Class[] { Object.class, Integer.TYPE });
		
		return setWithDepthMethod;
	}

	private Method resolveSetMethod() throws Exception {
		if (setMethod != null) return setMethod;
		
		setMethod = db.getClass().getMethod(setMethodName(), new Class[] { Object.class });
		
		return setMethod;
	}
	
	/** @sharpen.ignore */
	private String setMethodName() {
		return "set";
	}
	
	private Method setWithDepthMethod = null;
	private Method setMethod = null;
}
