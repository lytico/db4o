/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.freespace;

import java.io.*;

import com.db4o.internal.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;


public abstract class FileSizeTestCaseBase
	extends AbstractDb4oTestCase
	implements OptOutTA, OptOutInMemory {
    
    protected int databaseFileSize() {
        LocalObjectContainer localContainer = fixture().fileSession();
        localContainer.syncFiles();
        long length = new File(localContainer.fileName()).length();
        return (int)length;
    }
    
}
