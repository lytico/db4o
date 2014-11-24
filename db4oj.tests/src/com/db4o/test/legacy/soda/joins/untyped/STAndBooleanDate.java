/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.joins.untyped;

import java.util.*;

import com.db4o.test.legacy.soda.*;

public class STAndBooleanDate {
	
	public static transient SodaTest st;
	
	boolean shipped;
	Date dateOrdered;
	
	public STAndBooleanDate(){
	}
	
	public STAndBooleanDate(boolean shipped, int year, int month, int day){
		this.shipped = shipped;
		this.dateOrdered = new GregorianCalendar(year, month - 1, day).getTime();
	}
	
	public Object[] store() {
		return new Object[] {
			new STAndBooleanDate(false, 2002, 11, 1),
			new STAndBooleanDate(false, 2002, 12, 3),
			new STAndBooleanDate(false, 2002, 12, 5),
			new STAndBooleanDate(true, 2002, 11, 3),
			new STAndBooleanDate(true, 2002, 12, 4),
			new STAndBooleanDate(true, 2002, 12, 6)
			};
	}
	
	
	
	

}
