/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.conjunctions;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;

public class ConjunctionsTestSuite extends TestSuite{
    
    public Class[] tests(){
        return new Class[] {
            CJSingleField.class,
            CJChildField.class
        };
    }
    
    private static final int USED = -9999;

    public static void expect(Query q, int[] vals){
        ObjectSet objectSet = q.execute();
        while(objectSet.hasNext()){
            CJHasID cjs = (CJHasID)objectSet.next();
            boolean found = false;
            for (int i = 0; i < vals.length; i++) {
                if(cjs.getID() == vals[i]){
                    found = true;
                    vals[i] = USED;
                    break;
                }
            }
            Test.ensure(found);
        }
        for (int i = 0; i < vals.length; i++) {
            Test.ensure(vals[i] == USED);
        }
    }
    
}
