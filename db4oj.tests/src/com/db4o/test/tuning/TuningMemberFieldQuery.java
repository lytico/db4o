/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.test.tuning;

import com.db4o.*;
import com.db4o.query.*;
import com.db4o.test.*;


/**
 * Original performance on Carls machine with Skype on:
 * Querying 10000 objects for member identity: 500ms
 * @author root_rosenberger
 *
 */

public class TuningMemberFieldQuery {
    
    static final int COUNT = 2;
    
    TMFMember member;
    
    public TuningMemberFieldQuery(){
    }
    
    public TuningMemberFieldQuery(TMFMember member){
        this.member = member;
    }
	
	public void configure() {
        Db4o.configure().objectClass(this).objectField("member").indexed(true);
        Db4o.configure().objectClass(TMFMember.class).objectField("name").indexed(true);
	}
    
    public void store(){
        for (int i = 0; i < COUNT; i++) {
            Test.store(new TuningMemberFieldQuery(new TMFMember("" + i)));
        }
    }
    
    public void test(){
		Query q = Test.query();
		q.constrain(TuningMemberFieldQuery.class);
        
        q.descend("member").descend("name").constrain("1");
        
        
        
		long start = System.currentTimeMillis();
		ObjectSet objectSet = q.execute();
        
		long stop = System.currentTimeMillis();
        
        Test.ensure(objectSet.size() == 1);
        TuningMemberFieldQuery tmf = (TuningMemberFieldQuery)objectSet.next();
        Test.ensure(tmf.member.name.equals("1"));
        
		long duration = stop - start;
		System.out.println("Querying " + COUNT + " objects for member identity: " + duration + "ms");
    }
    
    public static class TMFMember{
        
        String name;
        
        public TMFMember(){
        }
        
        public TMFMember(String name){
            this.name = name;
        }
        
    }
    
}
