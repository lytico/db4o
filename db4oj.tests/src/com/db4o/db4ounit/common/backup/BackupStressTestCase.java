/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.backup;

import com.db4o.*;

public class BackupStressTestCase extends BackupStressTestCaseBase  {
    
	@Override
	protected void backup(ObjectContainer db, String fileName) {
		db.ext().backup(fileName);
	}

}
