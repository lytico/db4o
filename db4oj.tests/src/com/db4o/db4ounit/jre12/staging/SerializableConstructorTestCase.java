package com.db4o.db4ounit.jre12.staging;

import java.util.*;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * Jira #COR-1373
 */
public class SerializableConstructorTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new SerializableConstructorTestCase().runAll();
	}

	static class ExceptionalListHolder {

		public ExceptionalList<String> list;

		public ExceptionalListHolder(ExceptionalList<String> exceptionalList) {
			list = exceptionalList;
		}
		
	}
	
	static class ExceptionalList<E> extends ArrayList<E> {

		public ExceptionalList(String name) {
			if (null == name) {
				throw new IllegalArgumentException();
			}
		}
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		
	}
	
	@Override
	protected void store() throws Exception {
		store(new ExceptionalListHolder(new ExceptionalList<String>("foo")));
	}
	
	public void test() {
		ExceptionalListHolder instance = (ExceptionalListHolder) retrieveOnlyInstance(ExceptionalListHolder.class);
		Assert.isNotNull(instance.list);
	}
	
}
