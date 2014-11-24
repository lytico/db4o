/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.mocking;

import com.db4o.foundation.*;

import db4ounit.*;

public class CodeGenerator {
	
	/**
	 * Generates an array that can be used with {@link MethodCallRecorder#verify(MethodCall[])}.
	 * 
	 * Example:
	 * MethodCallRecorder recorder = new MethodCallRecorder();
	 * runTest(recorder);
	 * System.out.println(CodeGenerator.generateMethodCallArray(recorder))
	 * 
	 * @param calls MethodCall generator
	 * @return array string
	 */
	public static String generateMethodCallArray(Iterable4 calls) {
		final Iterable4 callStrings = Iterators.map(calls, new Function4() {
			public Object apply(Object arg) {
				return generateMethodCall((MethodCall)arg);
			}
		});
		return Iterators.join(callStrings.iterator(), "," + TestPlatform.NEW_LINE);
	}
	
	public static String generateValue(Object value) {
		if (value == null) {
			return "null";
		}
		if (value instanceof String) {
			return "\"" + value + "\"";
		}
		if (value instanceof Object[]) {
			return generateArray((Object[])value);
		}
		return value.toString();
	}
	
	public static String generateArray(Object[] array) {
		final Iterator4 values = Iterators.map(Iterators.iterate(array), new Function4() {
			public Object apply(Object arg) {
				return generateValue(arg);
			}
		});
		return "new Object[] " + Iterators.join(values, "{", "}", ", ");
	}

	public static String generateMethodCall(MethodCall call) {
		return "new MethodCall(\"" + call.methodName + "\", " + generateArray(call.args) + ")";
	}
}
