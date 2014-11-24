/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;


/**
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
class JDK_1_3 extends JDK_1_2{
	
	@decaf.Remove(decaf.Platform.JDK11)
	public final static class Factory implements JDKFactory {
		public JDK tryToCreate() {
	    	if(Reflection4.getMethod("java.lang.Runtime","addShutdownHook",
	            new Class[] { Thread.class }) == null){
	      		return null;
	      	}
	      	return new JDK_1_3();
		}
	}

	Thread addShutdownHook(Runnable runnable){
		Thread thread = new Thread(runnable, "Shutdown Hook");
	    Reflection4.invoke(Runtime.getRuntime(), "addShutdownHook", new Object[]{thread});
		return thread;
	}
	
	void removeShutdownHook(Thread thread){
	    Reflection4.invoke(Runtime.getRuntime(), "removeShutdownHook", new Object[]{thread});
	}
	
	public int ver(){
	    return 3;
	}
	
}
