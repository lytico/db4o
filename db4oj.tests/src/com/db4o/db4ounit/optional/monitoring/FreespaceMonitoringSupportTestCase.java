/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.optional.monitoring;

import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.internal.freespace.*;
import com.db4o.internal.slots.*;
import com.db4o.monitoring.*;

import db4ounit.*;
import db4ounit.extensions.OptOutNotSupportedJavaxManagement;
import db4ounit.extensions.fixtures.*;


@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class FreespaceMonitoringSupportTestCase extends MBeanTestCaseBase implements CustomClientServerConfiguration, OptOutNotSupportedJavaxManagement {
	
	public static class Item{
		
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		super.configure(config);
		config.add(new FreespaceMonitoringSupport());
	}

	public void configureClient(Configuration config) throws Exception {
		configure(config);
	}

	public void configureServer(Configuration config) throws Exception {
		configure(config);
	}
	
	public void test(){
		// ensure client is fully connected to the server already
		db().commit();
		assertMonitoredFreespaceIsCorrect();
		Item item = new Item();
		store(item);
		db().commit();
		assertMonitoredFreespaceIsCorrect();
		db().delete(item);
		db().commit();
		assertMonitoredFreespaceIsCorrect();
	}

	private void assertMonitoredFreespaceIsCorrect() {
		final IntByRef totalFreespace = new IntByRef();
		final IntByRef slotCount = new IntByRef();
		FreespaceManager freespaceManager = fileSession().freespaceManager();
		freespaceManager.traverse(new Visitor4<Slot>() {
			public void visit(Slot slot) {
				totalFreespace.value += slot.length();
				slotCount.value ++;
			}
		});
		Assert.areEqual(totalFreespace.value, totalFreespace());
		Assert.areEqual(slotCount.value, slotCount());
	}
	
	private Object totalFreespace() {
		return bean().getAttribute("TotalFreespace");
	}
	
	private Object slotCount() {
		return bean().getAttribute("SlotCount");
	}
	

	@Override
	protected Class<?> beanInterface() {
		return FreespaceMBean.class;
	}

	@Override
	protected String beanID() {
		return Db4oMBeans.mBeanIDForContainer(fileSession());
	}


}
