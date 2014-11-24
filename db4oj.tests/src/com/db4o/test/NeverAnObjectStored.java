/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class NeverAnObjectStored {
    
    public void test(){
        Query q = Test.query();
        q.constrain(this.getClass());
        ObjectSet objectSet = q.execute();
        Test.ensure(objectSet.size() == 0);
    }
    
}
