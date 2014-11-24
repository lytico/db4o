/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;


import com.db4o.*;
import com.db4o.config.*;

public class BTreeFreespaceManagerFixture extends Db4oSolo {
	
    protected ObjectContainer createDatabase(Configuration config) {
    	config.freespace().useBTreeSystem();
        return super.createDatabase(config);
    }

    public boolean accept(Class clazz) {
        return super.accept(clazz)
                && !OptOutBTreeFreespaceManager.class.isAssignableFrom(clazz);
    }

    public String label() {
        return "BTreeFreespace-" + super.label();
    }
}
