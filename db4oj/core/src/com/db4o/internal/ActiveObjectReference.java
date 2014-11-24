/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
class ActiveObjectReference extends java.lang.ref.WeakReference{
	
	Object _referent;
	
	ActiveObjectReference(Object queue, Object objectReference, Object obj){
		super(obj, (java.lang.ref.ReferenceQueue)queue) ;
		_referent = objectReference;
	}
}
