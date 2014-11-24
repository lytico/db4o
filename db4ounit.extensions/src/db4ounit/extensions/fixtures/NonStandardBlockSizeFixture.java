/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;


import com.db4o.*;
import com.db4o.config.*;

public class NonStandardBlockSizeFixture extends Db4oSolo {
	
    protected ObjectContainer createDatabase(Configuration config) {
    	config.blockSize(7);
        return super.createDatabase(config);
    }

    public boolean accept(Class clazz) {
        return super.accept(clazz)
                && !OptOutNonStandardBlockSize.class.isAssignableFrom(clazz);
    }

    public String label() {
        return "BlockSize-" + super.label();
    }
}
