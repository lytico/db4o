/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;

public class IsStored {
	
	String myString;
	
	public void test(){
		
		ObjectContainer con = Test.objectContainer();
		Test.deleteAllInstances(this);
		
		IsStored isStored = new IsStored();
		isStored.myString = "isStored";
		con.store(isStored);
		Test.ensure( con.ext().isStored(isStored) );
		Test.ensure( Test.occurrences(this) == 1 );
		con.delete(isStored);
		Test.ensure(! con.ext().isStored(isStored));
		Test.ensure( Test.occurrences(this) == 0 );
		con.commit();
		if(con.ext().isStored(isStored)){
			
			// this will fail in CS due to locally cached references
			if(! Test.clientServer){
				Test.error();
			}
			
		}
		Test.ensure( Test.occurrences(this) == 0 );
		con.store(isStored);
		Test.ensure( con.ext().isStored(isStored) );
		Test.ensure( Test.occurrences(this) == 1 );
		con.commit();
		Test.ensure( con.ext().isStored(isStored) );
		Test.ensure( Test.occurrences(this) == 1 );
		con.delete(isStored);
		Test.ensure( ! con.ext().isStored(isStored));
		Test.ensure( Test.occurrences(this) == 0 );
		con.commit();
		if(con.ext().isStored(isStored)){
			
			// this will fail in CS due to locally cached references
			if(! Test.clientServer){
				Test.error();
			}
		}
		Test.ensure( Test.occurrences(this) == 0 );
	}
	
}
