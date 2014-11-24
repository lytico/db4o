package com.db4o.internal;

import com.db4o.*;

public class Renames {

	public static Rename forField(String className, String name, String newName) {
    	return new Rename(className, name, newName);
    }

	public static Rename forClass(String name, String newName) {
    	return new Rename("", name, newName);
    }

	public static Rename forInverseQBE(Rename ren) {
    	return new Rename(ren.rClass, null, ren.rFrom);
    }

}
