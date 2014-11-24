/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * Renaming actions are stored to the database file to make 
 * sure that they are only performed once.
 * 
 * @exclude
 * @persistent
 */
public final class Rename implements Internal4
{
	public String rClass;
	public String rFrom;
	public String rTo;
	
	public Rename(){}
	
	public Rename(String aClass, String aFrom, String aTo){
		rClass = aClass;
		rFrom = aFrom;
		rTo = aTo;
	}
}
