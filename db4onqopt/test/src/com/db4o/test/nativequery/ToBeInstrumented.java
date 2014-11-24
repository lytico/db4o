package com.db4o.test.nativequery;

public class ToBeInstrumented extends com.db4o.query.Predicate<String> {
	public boolean match(String data) {
		return true;
	}
}
