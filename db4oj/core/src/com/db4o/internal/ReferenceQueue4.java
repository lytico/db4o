/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

/**
 * 
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
class ReferenceQueue4 extends java.lang.ref.ReferenceQueue
{
	ActiveObjectReference pollObjectReference(){
		return (ActiveObjectReference)super.poll();
	}
	
}
