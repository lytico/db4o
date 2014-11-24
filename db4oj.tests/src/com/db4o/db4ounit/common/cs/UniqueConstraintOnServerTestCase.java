package com.db4o.db4ounit.common.cs;


import com.db4o.config.*;
import com.db4o.constraints.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class UniqueConstraintOnServerTestCase extends Db4oClientServerTestCase implements CustomClientServerConfiguration{

    public static void main(String[] args) {
        new UniqueConstraintOnServerTestCase().runAll();
    }
    
    @Override
    protected void configure(Configuration config) throws Exception {
        config.objectClass(UniqueId.class).objectField("id").indexed(true);
        config.add(new UniqueFieldValueConstraint(UniqueId.class, "id"));
    }
    
	@Override
	public void configureServer(Configuration config) throws Exception {
		configure(config);
	}

	@Override
	public void configureClient(Configuration config) throws Exception {
		// do nothing
		
	}

    public void testWorksForUniqueItems() {
        store(new UniqueId(1));
        store(new UniqueId(2));
        store(new UniqueId(3));
        commit();
    }
    
    public void testNotUniqueItems() {
        store(new UniqueId(1));
        store(new UniqueId(1));
        boolean exceptionWasThrown = false;
        try {
            commit();
        } catch (UniqueFieldValueConstraintViolationException e) {
            exceptionWasThrown = true;
        }
        Assert.isTrue(exceptionWasThrown);
        db().rollback();
    }

    public static class UniqueId {
    	
        public int id;

        public UniqueId(int id) {
            this.id = id;
        }
    }

}
