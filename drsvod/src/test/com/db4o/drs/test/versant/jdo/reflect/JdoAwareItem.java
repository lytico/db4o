package com.db4o.drs.test.versant.jdo.reflect;

import java.util.*;

public class JdoAwareItem extends NotImplementedPersistenceCapable {
	
	
	public static class Meta {
		public static List<String> invocations = new ArrayList<String>();
	}

	public JdoAwareItem() {

	}

	public JdoAwareItem(String name) {
		this.name = name;
	}

	public JdoAwareItem(String name, int age) {
		this.name = name;
		this.age = age;
	}

	private String name;
	private int age;
	private static int staticField;
	private transient int transientField;

	private static String jdoFieldNames[] = { "name", "age" };
	private static Class jdoFieldTypes[] = { String.class, Integer.TYPE };

	private static String jdoGetname(JdoAwareItem obj) {
		Meta.invocations.add("jdoGetname");
		return obj.name;
	}

	private static void jdoSetname(JdoAwareItem obj, String name) {
		Meta.invocations.add("jdoSetname");
		obj.name = name;
	}

	private static int jdoGetage(JdoAwareItem obj) {
		Meta.invocations.add("jdoGetage");
		return obj.age;
	}

	private static void jdoSetage(JdoAwareItem obj, int age) {
		Meta.invocations.add("jdoSetage");
		obj.age = age;
	}

}
