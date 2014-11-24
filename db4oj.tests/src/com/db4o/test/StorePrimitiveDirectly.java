/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.ext.*;


public class StorePrimitiveDirectly {
    
    public void test(){
        
        ExtObjectContainer oc = Test.objectContainer();
        
        boolean exceptionOccurred = false;
        
        oc.configure().exceptionsOnNotStorable(false);

        try{
            oc.store(new Integer(1));
        }catch(ObjectNotStorableException onse){
            
           // happens now in test compbination but shouldn't
            
            exceptionOccurred = true;
            
        }
        
        // FIXME: 
        
        // Test.ensure(!exceptionOccurred);

        
        oc.configure().exceptionsOnNotStorable(true);
        
        exceptionOccurred = false;
        
        try{
            oc.store(new Integer(1));
        }catch(ObjectNotStorableException onse){
            exceptionOccurred = true;
        }
        
        
        // FIXME: 
        
        // Test.ensure(exceptionOccurred);
        
        oc.configure().exceptionsOnNotStorable(false);
    }

}
