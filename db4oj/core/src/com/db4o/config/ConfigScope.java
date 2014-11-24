/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.config;

import java.io.*;

import com.db4o.foundation.*;

/**
 * Defines a scope of applicability of a config setting.<br><br>
 * Some of the configuration settings can be either: <br><br>
 * - enabled globally; <br>
 * - enabled individually for a specified class; <br>
 * - disabled.<br><br>
 * @see com.db4o.config.FileConfiguration#generateUUIDs(ConfigScope)
 */
public final class ConfigScope implements Serializable {

	public static final int DISABLED_ID = -1;
	public static final int INDIVIDUALLY_ID = 1;
	public static final int GLOBALLY_ID = Integer.MAX_VALUE;

	private static final String DISABLED_NAME="disabled";
	private static final String INDIVIDUALLY_NAME="individually";
	private static final String GLOBALLY_NAME="globally";
	
	/**
	 * Marks a configuration feature as globally disabled.
	 */
	public static final ConfigScope DISABLED = new ConfigScope(DISABLED_ID,DISABLED_NAME);

	/**
	 * Marks a configuration feature as individually configurable.
	 */
	public static final ConfigScope INDIVIDUALLY = new ConfigScope(INDIVIDUALLY_ID,INDIVIDUALLY_NAME);

	/**
	 * Marks a configuration feature as globally enabled.
	 */
	public static final ConfigScope GLOBALLY = new ConfigScope(GLOBALLY_ID,GLOBALLY_NAME);

	private final int _value;
	private final String _name;
	
	private ConfigScope(int value,String name) {
		_value=value;
		_name=name;
	}

	/**
	 * Checks if the current configuration scope is globally
	 * enabled or disabled. 
	 * @param defaultValue - default result 
	 * @return false if disabled, true if globally enabled, default 
	 * value otherwise
	 */
	public boolean applyConfig(TernaryBool defaultValue) {
		switch(_value) {
			case DISABLED_ID:
				return false;
			case GLOBALLY_ID: 
				return !defaultValue.definiteNo();
			default:
				return defaultValue.definiteYes();
		}
	}
	
	public boolean equals(Object obj) {
		if(this==obj) {
			return true;
		}
		if(obj==null||getClass()!=obj.getClass()) {
			return false;
		}
		ConfigScope tb=(ConfigScope)obj;
		return _value==tb._value;
	}
	
	public int hashCode() {
		return _value;
	}
	
	private Object readResolve() {
		switch(_value) {
		case DISABLED_ID:
			return DISABLED;
		case INDIVIDUALLY_ID:
			return INDIVIDUALLY;
		default:
			return GLOBALLY;
		}
	}
	
	public String toString() {
		return _name;
	}
}
