/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 * @persistent
 */
public class User implements Internal4{
	public String name;
	public String password;
	
	public User() {
	}
	
	public User(String name_, String password_) {
		name = name_;
		password = password_;
	}
}

