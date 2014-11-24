/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.cobra;

import com.db4o.drs.test.versant.*;
import com.db4o.drs.versant.*;
import com.db4o.drs.versant.metadata.*;

public class CobraReplicationSupportTestCase extends VodCobraTestCaseBase {
	
	public void testInitializeTwice(){
		CobraReplicationSupport.initialize(_cobra);
		CobraReplicationSupport.initialize(_cobra);
	}
	
	public void testInitializeTimestamp(){
		CobraReplicationSupport.intializeTimestamp(_cobra);
		// test single instance is present, otherwise the following method throws
		_cobra.from(TimestampToken.class).single();
	}

}
