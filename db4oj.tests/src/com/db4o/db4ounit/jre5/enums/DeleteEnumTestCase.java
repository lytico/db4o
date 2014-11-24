/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.enums;

import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class DeleteEnumTestCase extends AbstractDb4oTestCase {
    
	public static final class Item {
		public TypeCountEnum a;
	}
	
	@Override
	protected void configure(Configuration config) {
		config.objectClass(Item.class).cascadeOnDelete(true);
	}
    
    protected void store(){
        for (int i = 0; i < 2; i++) {
            Item item = new Item();
            item.a = TypeCountEnum.A;
            db().store(item);
        }
    }
    
    public void test() throws Exception{
        Item item = queryOne();
        db().delete(item);
        reopen();
        item = queryOne();
        Assert.areEqual(TypeCountEnum.A, item.a);
    }
    
    private Item queryOne(){
        Query q = newQuery(Item.class);
        return (Item)q.execute().next();
    }
    
}
