/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant.timestamp;

import com.db4o.drs.test.versant.*;
import com.db4o.drs.versant.*;
import com.db4o.drs.versant.timestamp.*;

import db4ounit.*;

public class TimestampGeneratorTestCase extends VodCobraTestCaseBase {
	
	public void test(){
		CobraReplicationSupport.initialize(_cobra);
		TimestampGenerator timestampGenerator = new TimestampGenerator(_cobra);
		long timestamp = timestampGenerator.generate();
		Assert.isGreater(0, timestamp);
		long timestamp2 = timestampGenerator.generate();
		Assert.isGreater(timestamp, timestamp2);
	}

}
