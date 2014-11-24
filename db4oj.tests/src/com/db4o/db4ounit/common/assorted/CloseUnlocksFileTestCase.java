/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import java.io.*;

import com.db4o.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.foundation.io.*;

import db4ounit.*;


public class CloseUnlocksFileTestCase extends Db4oTestWithTempFile {
    
	/**
	 * @deprecated
	 */
    public void test(){
        File4.delete(tempFile());
        Assert.isFalse(exists(tempFile()));
        ObjectContainer oc = Db4oEmbedded.openFile(newConfiguration(), tempFile());
        oc.close();
        File4.delete(tempFile());
        Assert.isFalse(exists(tempFile()));
    }

	private boolean exists(final String fileName) {
		return new File(fileName).exists();
	}

}
