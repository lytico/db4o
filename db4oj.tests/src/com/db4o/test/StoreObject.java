/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.tools.*;

public class StoreObject {

	Object _field;
	
	public void storeOne()	{
		_field = new Object();
	}
	
	public void testOne() {
        
        Db4o.configure().objectClass(Object.class).cascadeOnActivate(true);
        
        
        Query q = Test.query();
        q.constrain(new Object());
        q.execute();
        
        Test.close();
        
        Statistics.main(new String[]{Test.FILE_SOLO});
        
        Test.reOpen();
        
        q = Test.query();
        StoreObject template = new StoreObject();
        template._field = new Object(); 
        q.constrain(template);
        q.execute();
        
        Test.close();
        
        Statistics.main(new String[]{Test.FILE_SOLO});
        
        Test.reOpen();


        
        
        
		// Test.ensure(_field != null);
	}
}
