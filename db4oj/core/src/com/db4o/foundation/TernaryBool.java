/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

import java.io.*;

/**
 * yes/no/dontknow data type
 * 
 * @exclude
 */
public final class TernaryBool implements Serializable {

	private static final int NO_ID = -1;
	private static final int YES_ID = 1;
	private static final int UNSPECIFIED_ID = 0;

	public static final TernaryBool NO = new TernaryBool(NO_ID);
	public static final TernaryBool YES = new TernaryBool(YES_ID);
	public static final TernaryBool UNSPECIFIED = new TernaryBool(UNSPECIFIED_ID);

	private final int _value;
	
	private TernaryBool(int value) {
		_value=value;
	}

	public boolean booleanValue(boolean defaultValue) {
		switch(_value) {
			case NO_ID:
				return false;
			case YES_ID: 
				return true;
			default:
				return defaultValue;
		}
	}
	
	public boolean isUnspecified() {
		return this==UNSPECIFIED;
	}

	public boolean definiteYes() {
		return this==YES;
	}

	public boolean definiteNo() {
		return this==NO;
	}

	public static TernaryBool forBoolean(boolean value) {
		return (value ? YES : NO);
	}
	
	public boolean equals(Object obj) {
		if(this==obj) {
			return true;
		}
		if(obj==null||getClass()!=obj.getClass()) {
			return false;
		}
		TernaryBool tb=(TernaryBool)obj;
		return _value==tb._value;
	}
	
	public int hashCode() {
		return _value;
	}
	
	private Object readResolve() {
		switch(_value) {
			case NO_ID:
				return NO;
			case YES_ID:
				return YES;
			default:
				return UNSPECIFIED;
		}
	}
	
	public String toString() {
		switch(_value) {
		case NO_ID:
			return "NO";
		case YES_ID:
			return "YES";
		default:
			return "UNSPECIFIED";
	}
	}
}
