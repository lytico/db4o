/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.enums;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class EnumUpdateTestCase extends AbstractDb4oTestCase {
	
	public enum MyEnum {
		
	    A("A"),
	    B("B");
	    
	    private String _name;
	    
	    private boolean _modified;
	    
	    private MyEnum(String name){
	    	_name = name;
	    }
	    
	    public void modify(){
	    	_modified = true;
	    }
	    
	    public boolean isModified(){
	    	return _modified;
	    }
	}
	
	public void test(){
		db().store(MyEnum.A);
		MyEnum.A.modify();
		db().store(MyEnum.A);
		db().commit();
		MyEnum committedMyEnumA = db().peekPersisted(MyEnum.A, Integer.MAX_VALUE, true);
		Assert.areNotSame(MyEnum.A, committedMyEnumA);
		Assert.isTrue(committedMyEnumA.isModified());
	}
	
	public static void main(String[] args) {
		new EnumUpdateTestCase().runSolo();
	}

}
