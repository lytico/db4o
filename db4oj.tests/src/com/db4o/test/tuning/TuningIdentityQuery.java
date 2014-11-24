/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.tuning;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;


/**
 * Original performance on the server:
 * Querying 10000 objects for member identity: 640ms
 * 
 * Original performance on Carls machine with Skype on:
 * Querying 10000 objects for member identity: 703ms
 * 
 * Result after enabling member indices:
 * Querying time too small to measure. 
 */

public class TuningIdentityQuery {
    
    static final int COUNT = 10000;
    
    TIMember member;
    
    public TuningIdentityQuery(){
    }
    
    public TuningIdentityQuery(TIMember member){
        this.member = member;
    }
	
	public void configure() {
		Db4o.configure().objectClass(this).objectField("member").indexed(true);
	}
    
    public void store(){
        for (int i = 0; i < COUNT; i++) {
            Test.store(new TuningIdentityQuery(new TIMember("" + i)));
        }
    }
    
    public void test(){
		Query q = Test.query();
		q.constrain(TIMember.class);
		ObjectSet objectSet = q.execute();
		TIMember member = (TIMember) objectSet.next();
		q = Test.query();
		q.constrain(TuningIdentityQuery.class);
		q.descend("member").constrain(member).identity();
        
		long start = System.currentTimeMillis();
		objectSet = q.execute();
        
		long stop = System.currentTimeMillis();
        
        Test.ensure(objectSet.size() == 1);
        TuningIdentityQuery ti = (TuningIdentityQuery)objectSet.next();
        Test.ensure(ti.member == member);
        
		long duration = stop - start;
		System.out.println("Querying " + COUNT + " objects for member identity: " + duration + "ms");
    }
    
    public static class TIMember{
        
        String name;
        
        public TIMember(){
            
        }
        
        public TIMember(String name){
            this.name = name;
        }
        
    }
    

}
