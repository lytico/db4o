/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeDeleteFalse extends AbstractDb4oTestCase {

    public static class CascadeDeleteFalseHelper{  
    }
    
    public CascadeDeleteFalseHelper h1;
    public CascadeDeleteFalseHelper h2;
    public CascadeDeleteFalseHelper h3;
    
    protected void configure(Configuration conf) {
        conf.objectClass(this).cascadeOnDelete(true);
        conf.objectClass(this).objectField("h3").cascadeOnDelete(false);
    }
    
    protected void store() {
    	CascadeDeleteFalse cdf = new CascadeDeleteFalse();
        cdf.h1 = new CascadeDeleteFalseHelper();
        cdf.h2 = new CascadeDeleteFalseHelper();
        cdf.h3 = new CascadeDeleteFalseHelper();
        db().store(cdf);
    }
    
    public void test() {
        checkHelperCount(3);
        
        CascadeDeleteFalse cdf = (CascadeDeleteFalse)retrieveOnlyInstance(getClass());
        db().delete(cdf);
        
        checkHelperCount(1);
    }
    
    private void checkHelperCount (int count){
        Assert.areEqual(count, countOccurences(CascadeDeleteFalseHelper.class));
    }
    
    public static void main(String[] args) {
    	new CascadeDeleteFalse().runSolo();
	}
    
}
