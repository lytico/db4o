/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;
import com.db4o.foundation.io.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.fixtures.*;


public class SystemInfoTestCase extends Db4oTestWithTempFile implements OptOutNoFileSystemData {

	private ObjectContainer _db;
	
    public static class Item {
        
    }
    
    public static void main(String[] arguments) {
        new ConsoleTestRunner(SystemInfoTestCase.class).run();
    }

    public void setUp() throws Exception {
    	_db = Db4oEmbedded.openFile(newConfiguration(), tempFile());
    }
    
    @Override
    public void tearDown() throws Exception {
    	close();
    	super.tearDown();
    }

	private void close() {
		if (_db != null) {
			_db.close();
			_db = null;
		}
	}
    
    public void testDefaultFreespaceInfo(){
        assertFreespaceInfo(fileSession().systemInfo());
    }
    
    private LocalObjectContainer fileSession() {
		return (LocalObjectContainer) db();
	}

	private ExtObjectContainer db() {
		return _db.ext();
	}

	private void assertFreespaceInfo(SystemInfo info){
        Assert.isNotNull(info);
        Item item = new Item();
        db().store(item);
        db().commit();
        db().delete(item);
        db().commit();
        Assert.isTrue(info.freespaceEntryCount() > 0);
        Assert.isTrue(info.freespaceSize() > 0);
    }

    public void testTotalSize(){
        long actual = db().systemInfo().totalSize();
        close();
            
		long expectedSize = File4.size(tempFile());
        Assert.areEqual(expectedSize, actual);
    }
}
