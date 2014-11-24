/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import java.util.*;

import com.db4o.drs.inside.*;
import com.db4o.drs.test.*;
import com.db4o.drs.versant.*;

public class VodCobraDrsFixture implements DrsProviderFixture{
	
	private static final String USER_NAME = "drs";
	
	private static final String PASSWORD = "drs";
	
	private VodDatabase _vod;
	
	protected VodCobraReplicationProvider _provider;

	private final String _name;
	
	public VodCobraDrsFixture(String name){
		_name = name;
		init();
	}

	private void init() {
		_vod = new VodDatabase(_name, USER_NAME, PASSWORD);
		_vod.removeDb();
		_vod.produceDb();
		_vod.addUser();
		
		Set<Package> packages = new HashSet<Package>();
		for (Class clazz : DrsTestCase.mappings) {
			Package p = clazz.getPackage();
			if (!packages.add(p)) {
				continue;
			}
			_vod.addJdoMetaDataFile(p);
		}
		
		ensureJdoMetadataCreated();
	}

	private void ensureJdoMetadataCreated() {
		VodJdo.createInstance(_vod).close();
	}
	
	public void close() {
		_provider.destroy();		
		_provider = null;
	}

	public void open() {
		_provider = new VodCobraReplicationProvider(_vod);
	}

	public TestableReplicationProviderInside provider() {
		return _provider;
	}
	
	@Override
	public String toString() {
		return this.getClass().getSimpleName() + " " + _vod;
	}
	
	public void clean() {
		internalClean(true);
	}

	private void internalClean(boolean deleteAll) {
		if(deleteAll){
			VodCobra.deleteAll(_vod);
		}
	}
	
	public void destroy(){
		internalClean(false);
		_vod.stopEventDriver();
		_vod.removeDb();
	}
	

}
