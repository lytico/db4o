/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.ext.*;

public class NoInternalClasses {
	
	public void store(){
        try{
            Test.store(new StaticClass());
        }catch(ObjectNotStorableException onse){
            
            // a possible exception
            
        }
	}
	
	public void test(){
		Test.ensureOccurrences(new StaticClass(),0);
	}

}
