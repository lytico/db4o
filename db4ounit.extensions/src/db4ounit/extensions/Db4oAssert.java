/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions;

import com.db4o.ext.*;

import db4ounit.*;


public class Db4oAssert {

    public static void persistedCount(int expected, Class extent) {
        Assert.areEqual(expected, db().query(extent).size());
    }
    
    private static ExtObjectContainer db(){
        return fixture().db();
    }

    private static Db4oFixture fixture() {
        return Db4oFixtureVariable.fixture();
    }
    
    
    
    

}
