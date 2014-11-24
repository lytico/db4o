/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import java.util.*;

import com.db4o.drs.versant.*;

import db4ounit.*;

public class VodStandaloneEventProcessorTestCase implements TestCase {
	
	protected static final String DATABASE_NAME = "Standalone";
	
	public void test(){
		VodDatabase _vod = new VodDatabase(DATABASE_NAME, new Properties());
		_vod.produceDb();
		_vod.createEventSchema();
		_vod.startEventDriver();
		_vod.startEventProcessor();
		_vod.stopEventProcessor();
		_vod.stopEventDriver();
		_vod.removeDb();
	}
	

}
