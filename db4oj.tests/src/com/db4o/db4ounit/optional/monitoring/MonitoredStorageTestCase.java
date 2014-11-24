/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional.monitoring;

import javax.management.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.io.*;
import com.db4o.monitoring.*;

import db4ounit.*;
import db4ounit.extensions.OptOutNotSupportedJavaxManagement;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class MonitoredStorageTestCase implements TestLifeCycle, OptOutNotSupportedJavaxManagement {
	
	private CountingStorage _storage = new CountingStorage(new MemoryStorage());
	
	private EmbeddedObjectContainer _container;

	private final MBeanProxy _bean = new MBeanProxy(getIOMBeanName());
	
	public void testNumSyncsPerSecond() {
		Assert.areEqual(_storage.numberOfSyncCalls(), getAttribute("SyncsPerSecond"));		
	}
	
	public void testNumBytesReadPerSecond() {
		Assert.areEqual(_storage.numberOfBytesRead(), getAttribute("BytesReadPerSecond"));		
	}

	public void testNumBytesWrittenPerSecond() {
		Assert.areEqual(_storage.numberOfBytesWritten(), getAttribute("BytesWrittenPerSecond"));		
	}

	public void testNumReadsPerSecond() {
		Assert.areEqual(_storage.numberOfReadCalls(), getAttribute("ReadsPerSecond"));		
	}

	public void testNumWritesPerSecond() {
		Assert.areEqual(_storage.numberOfWriteCalls(), getAttribute("WritesPerSecond"));		
	}

	public void setUp() throws Exception{
		ClockMock clock = new ClockMock();
		
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(_storage);
		config.common().add(new IOMonitoringSupport());
		config.common().environment().add(clock);
		
		_container = Db4oEmbedded.openFile(config, "");
		_container.store(new Object());
		_container.commit();
		
		clock.advance(1000);
	
	}

	public void tearDown() throws Exception {
		if(null != _container){
			_container.close();
		}
	}

	private double getAttribute(final String attribute) {
		return _bean.<Double>getAttribute(attribute);
	}

	
	private ObjectName getIOMBeanName() {
		return Db4oMBeans.mBeanNameFor(IOMBean.class, "");
	}

}
