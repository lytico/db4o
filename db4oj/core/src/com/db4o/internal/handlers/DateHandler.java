/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.handlers;

import java.util.*;

/**
 * @sharpen.ignore
 */
public class DateHandler extends DateHandlerBase {
	
	private static final Date DEFAULTVALUE = new Date(0);
	
	public Object defaultValue(){
		return DEFAULTVALUE;
	}

	public Object primitiveNull(){
		return null;
	}

	public Object nullRepresentationInUntypedArrays() {
	    return new Date(0);
	}
	
	public Object copyValue(Object from, Object to) {
		try{
			((Date)to).setTime(((Date)from).getTime());
		}catch(Exception e){
//			e.printStackTrace();
		}
		return to;
	}
}
