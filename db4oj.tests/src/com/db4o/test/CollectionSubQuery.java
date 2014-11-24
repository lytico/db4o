/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class CollectionSubQuery {
	private final static String ID="X";
	
	public static class Data {
		public String id;

		public Data(String id) {
			this.id = id;
		}
	}
	
	public List list;
	
    public void storeOne(){
    	this.list=new ArrayList();
    	this.list.add(new Data(ID));
    }
    
    public void test(){
        Query q = Test.query();
        q.constrain(CollectionSubQuery.class);
        Query sub=q.descend("list");
        // Commenting out this constraint doesn't effect behavior
        sub.constrain(Data.class);
        // If this subsub constraint is commented out, the result
        // contains a Data instance as expected. With this constraint,
        // we get the containing ArrayList.
        Query subsub=sub.descend("id");
        subsub.constrain(ID);
        ObjectSet result=sub.execute();
        Test.ensure(result.size()==1);
        Test.ensure(result.next().getClass()==Data.class);
    }
    
    public static void main(String[] args) {
		AllTests.run(CollectionSubQuery.class);
	}
}
