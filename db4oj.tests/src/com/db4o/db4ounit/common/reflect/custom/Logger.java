package com.db4o.db4ounit.common.reflect.custom;

public class Logger {

	public static void log(String s) {
//		System.err.println(s);
	}

	public static void logMethodCall(Object target, String methodName) {
		log(target.toString() + "." + methodName + "()");
	}

	public static void logMethodCall(Object target, String methodName, Object arg) {
		log(target.toString() + "." + methodName + "(" + arg + ")");
	}

	public static void logMethodCall(Object target, String methodName, Object arg1, Object arg2) {
		log(target.toString() + "." + methodName + "(" + arg1 + ", " + arg2 + ")");
	}

}
