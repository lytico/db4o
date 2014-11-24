/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import java.io.*;

import com.db4o.drs.test.versant.data.*;
import com.db4o.drs.versant.*;
import com.db4o.drs.versant.eventprocessor.*;
import com.versant.event.*;
import com.versant.odbms.*;
import com.versant.odbms.model.*;

import db4ounit.*;

public class VodEventTestCase extends VodProviderTestCaseBase {	
	
	private final class LoggingExceptionListener implements ExceptionListener {
		public void exceptionOccurred (Throwable exception){
		    exception.printStackTrace ();
		}
	}

	// Doesn't work: VOD throws ClassCastException when
	// trying to get system schema classes.
	public void _testEventSchemaCreation(){
		
		// The following now happens in classSetup() anyway, so we don't have to do it here.
		// _vod.createEventSchema();
		
		DatastoreManager dm = _vod.createDatastoreManager();
		DatastoreInfo info = dm.getPrimaryDatastoreInfo();
		SchemaEditor editor = dm.getSchemaEditor();
		boolean systemClasses = true;
		long[] classLoids = dm.locateAllClasses(info, systemClasses);
		boolean found = false;
		for (int i = 0; i < classLoids.length; i++) {
			DatastoreSchemaClass dc = editor.findClass(classLoids[i], info);
			if("VEDChannel".equals(dc.getName())){
				found = true;
			}
		}
		dm.close();
		Assert.isTrue(found);
	}
	
	public void testEventDriverStartAndStop() throws IOException {
		VodEventDriver eventDriver = new VodEventDriver(_vod.eventConfiguration());
		Assert.isTrue(eventDriver.start());
		eventDriver.stop();
	}
	
	public void testSimpleEvent() throws Exception {
		EventClient client = newEventClient();
		try{
	        ClassChannelBuilder builder = new ClassChannelBuilder (_cobra.schemaName(Item.class.getName()));
	        EventChannel channel = client.newChannel ("item", builder);
		    RecordingEventListener eventListener = new RecordingEventListener(VodEvent.CREATED, VodEvent.MODIFIED);
		    channel.addVersantEventListener (eventListener);
		    Item item = storeAndCommitItem();
			updateItem(item);
			eventListener.verify(10000);
		} finally {
			client.shutdown();
		}
	}
	

	private void updateItem(Item item) {
		item.name("newName");
		_jdo.commit();
	}

	private Item storeAndCommitItem() {
		Item item = new Item("two");
		_jdo.store(item);
		_jdo.commit();
		return item;
	}

	private EventClient newEventClient() throws IOException {
		EventClient client = VodEventClient.newEventClient(_vod.eventConfiguration());
		client.addExceptionListener (new LoggingExceptionListener());
		return client;
	}

}
