/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import java.lang.reflect.*;

import com.db4o.db4ounit.common.handlers.*;
import com.db4o.ext.*;

import db4ounit.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class MsgProtocolOrderTestCase extends FormatMigrationTestCaseBase{
	
	@Override
	protected void assertObjectsAreReadable(ExtObjectContainer objectContainer) {
		// do nothing
	}

	@Override
	protected String fileNamePrefix() {
		return "csprotocol";
	}

	@Override
	protected void store(ObjectContainerAdapter adapter) {
		if(db4oMajorVersion() < 8){
			return;
		}
		ClassLoader classLoader = adapter.objectContainer().getClass().getClassLoader();
		ClassLoader parentClassLoader = classLoader.getParent();
		if(! classLoader.getClass().getSimpleName().contains("VersionClassLoader")){
			System.err.println("Not in a versioned environment. Ignoring test " + MsgProtocolOrderTestCase.class.getSimpleName());
			return;
		}
		Class<?> clazz = null;
		Class classInParentClassLoader = null;
		try {
			clazz = classLoader.loadClass("com.db4o.cs.internal.messages.Msg");
			classInParentClassLoader = parentClassLoader.loadClass("com.db4o.cs.internal.messages.Msg");
		} catch (ClassNotFoundException e) {
			Assert.fail("Msg class not found.", e);
		}
		if(clazz == classInParentClassLoader){
			Assert.fail("CS Protocol class Msg not found in archive.");
		}
		Object[] messages = messagesFieldValue(clazz);
		Object[] messagesInMainClassLoader = messagesFieldValue(classInParentClassLoader);
		for (int i = 0; i < messages.length; i++) {
			if(messages[i] != null){
				if(messagesInMainClassLoader[i] == null){
					Assert.fail("Msg.messages inconsistent for message " + i + ". Message " + messages[i].getClass().getSimpleName() +  " seems to have been removed.");
				}
				if(! messages[i].getClass().getSimpleName().equals(messagesInMainClassLoader[i].getClass().getSimpleName())){
					Assert.fail("Msg.messages inconsistent for message " + i + " class:" + messages[i].getClass().getSimpleName());
				}
			}
		}
	}

	private Object[] messagesFieldValue(Class<?> clazz) {
		Object[] messages = null;
		try {
			Field field = clazz.getDeclaredField("_messages");
			field.setAccessible(true);
			messages = (Object[]) field.get(null);
		} catch (Exception e) {
			Assert.fail("Field _messages not found.", e);
		} 
		return messages;
	}

}
