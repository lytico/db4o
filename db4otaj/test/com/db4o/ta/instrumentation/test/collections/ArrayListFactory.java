package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

public class ArrayListFactory {

	public List createArrayList() {
		return new ArrayList();
	}

	public List createSizedArrayList() {
		return new ArrayList(42);
	}

	public List createNestedArrayList() {
		return new ArrayList(new ArrayList(42));
	}

	public List createMethodArgArrayList() {
		return new ArrayList(size());
	}

	public List createConditionalArrayList() {
		return new ArrayList(size() > 41 ? size() : 42);
	}

	private int size() {
		return 42;
	}
}
