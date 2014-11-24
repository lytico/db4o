/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.ext.*;


public class InvalidUUID {
    
    public String name;
    
    public void configure(){
        Db4o.configure().objectClass(this.getClass()).generateUUIDs(true);
    }
    
    public void storeOne(){
        name = "theOne";
    }
    
    public void testOne(){
        ExtObjectContainer oc = Test.objectContainer();
        
        Db4oUUID myUuid = oc.getObjectInfo(this).getUUID();
        
        Test.ensure(myUuid != null);
        
        byte[] mySignature = myUuid.getSignaturePart();
        long myLong = myUuid.getLongPart();
        
        long unknownLong = Long.MAX_VALUE - 100;  
        byte[] unknownSignature = new byte[]{1,2,4,99,33,22};
       
        Db4oUUID unknownLongPart= new Db4oUUID(unknownLong, mySignature);
        Db4oUUID unknownSignaturePart = new Db4oUUID(myLong, unknownSignature);
        Db4oUUID unknownBoth = new Db4oUUID(unknownLong, unknownSignature);
        
        Test.ensure(oc.getByUUID(unknownLongPart) == null);
        Test.ensure(oc.getByUUID(unknownSignaturePart) == null);
        Test.ensure(oc.getByUUID(unknownBoth) == null);
        
        
        Test.ensure(oc.getByUUID(unknownLongPart) == null);
        
        Test.delete(this);
        Test.commit();
        
        Test.ensure(oc.getByUUID(myUuid) == null);
    }

}
