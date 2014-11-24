/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.foundation;

import com.db4o.ext.*;

public class StackAnalyzer {
	
	public static String methodCallAboveAsString(Class<?> clazz) {
		return methodCallAboveAsStringInternal(clazz.getName());
	}
	
	public static String methodCallAsString(int depth){
		return methodCallAsStringInternal(depth);
	}
	
	/**
	 * @sharpen.remove "stack analyis NA"
	 */
	private static String methodCallAboveAsStringInternal(String className){
		StackTraceElement[] stackTrace = Thread.currentThread().getStackTrace();
		boolean foundFirst = false;
		for (int i = 0; i < stackTrace.length; i++) {
			if(className.equals(stackTrace[i].getClassName())){
				foundFirst = true;
			} else {
				if (foundFirst){
					return methodFrameAsString(stackTrace[i]);
				}
			}
		}
		throw new IllegalArgumentException("Classname " + className + " not found in stack."); 
	}
	
	/**
	 * @sharpen.remove "stack analyis NA"
	 */
	private static String methodCallAsStringInternal(int depth){
		int callerDepth = depth + 2;
		StackTraceElement[] stackTrace = Thread.currentThread().getStackTrace();
		if(callerDepth > stackTrace.length){
			throw new IllegalStateException("depth to high max:" + stackTrace.length + " requested:" + depth);
		}
		StackTraceElement stackTraceElement = stackTrace[callerDepth];
		return methodFrameAsString(stackTraceElement);
	}

	/**
	 * @sharpen.remove
	 */
	private static String methodFrameAsString(StackTraceElement stackTraceElement) {
		try {
			return Class.forName(stackTraceElement.getClassName()).getSimpleName() + "#" + stackTraceElement.getMethodName() + "()";
		} catch (ClassNotFoundException e) {
			// What? The class we are calling from is not available? Hm, we are not on Java. 
			throw new Db4oException(e);
		}
	}


}
