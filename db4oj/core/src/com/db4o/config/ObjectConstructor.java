/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.config;

import com.db4o.ObjectContainer;

/**
 * interface to allow instantiating objects by calling specific constructors.
 * <br><br>
 * By writing classes that implement this interface, it is possible to
 * define which constructor is to be used during the instantiation of a stored object.
 * <br><br>
 * Before starting a db4o session, translator classes that implement the 
 * ObjectConstructor or
 * {@link ObjectTranslator ObjectTranslator}
 * need to be registered.<br><br>
 * Example:<br>
 * <code>
 * EmbeddedConfiguration config = Db4oEmbedded.newConfiguration(); <br>
 * ObjectClass oc = config.common().objectClass("package.className");<br>
 * oc.translate(new FooTranslator());</code><br><br>
 */
public interface ObjectConstructor extends ObjectTranslator {

	/**
	 * db4o calls this method when a stored object needs to be instantiated.
	 * <br><br>
	 * @param container the ObjectContainer used
	 * @param storedObject the object stored with 
	 * {@link ObjectTranslator#onStore ObjectTranslator.onStore}.
	 * @return the instantiated object.
	 */
	public Object onInstantiate(ObjectContainer container, Object storedObject);
	
}