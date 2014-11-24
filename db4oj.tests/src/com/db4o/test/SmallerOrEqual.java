/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class SmallerOrEqual {

    public int val;

    public SmallerOrEqual() {

    }

    public SmallerOrEqual(int val) {
        this.val = val;
    }

    public void store() {
        Test.store(new SmallerOrEqual(1));
        Test.store(new SmallerOrEqual(2));
        Test.store(new SmallerOrEqual(3));
        Test.store(new SmallerOrEqual(4));
        Test.store(new SmallerOrEqual(5));
    }

    public void test() {
        int[] expect = {1,2,3};
		Query q = Test.query();
		q.constrain(SmallerOrEqual.class);
		q.descend("val").constrain(new Integer(3)).smaller().equal();
		ObjectSet res = q.execute();
		while(res.hasNext()){
		    SmallerOrEqual r = (SmallerOrEqual)res.next();
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
