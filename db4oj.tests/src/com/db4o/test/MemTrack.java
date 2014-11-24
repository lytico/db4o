/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

public class MemTrack {
    
    static String bigString;
    static int counter;
    
    public void configure(){
        if(bigString == null){
            StringBuffer sb = new StringBuffer();
            for(int i = 0; i < 10000; i ++){
                sb.append(i);
            }
            bigString = sb.toString();
        }
    }
    
    
	
	public void test(){
		Test.deleteAllInstances(Atom.class);
		Test.store(new Atom(bigString));
	}

}

