/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class GreaterOrEqual {

    public int val;

    public GreaterOrEqual() {

    }

    public GreaterOrEqual(int val) {
        this.val = val;
    }

    public void store() {
        Test.store(new GreaterOrEqual(1));
        Test.store(new GreaterOrEqual(2));
        Test.store(new GreaterOrEqual(3));
        Test.store(new GreaterOrEqual(4));
        Test.store(new GreaterOrEqual(5));
    }

    public void test() {
        int[] expect = {3,4,5};
		Query q = Test.query();
		q.constrain(GreaterOrEqual.class);
		q.descend("val").constrain(new Integer(3)).greater().equal();
		ObjectSet res = q.execute();
		while(res.hasNext()){
		    GreaterOrEqual r = (GreaterOrEqual)res.next();
		    for (int i = 0; i < expect.length; i++) {
		        if(expect[i] == r.val){
		            expect[i] = 0;
		        }
            }
		}
		for (int i = 0; i < expect.length; i++) {
		    Test.ensure(expect[i] == 0);
		}
    }

}
