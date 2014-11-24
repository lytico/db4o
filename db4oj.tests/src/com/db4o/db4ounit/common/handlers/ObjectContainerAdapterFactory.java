/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.handlers;

/**
 * @exclude
 */
public class ObjectContainerAdapterFactory {
	public static final ObjectContainerAdapter PRE7_1_FACADE = new Pre7_1ObjectContainerAdapter(); 
	public static final ObjectContainerAdapter POST7_1_FACADE = new Post7_1ObjectContainerAdapter();
	
	public static ObjectContainerAdapter forVersion(int major, int minor) {
		if ( (major == 7 && minor >= 1) || major > 7 ) {
        	return POST7_1_FACADE;
        }
        else {
        	return PRE7_1_FACADE;
        }        		
	}
}
