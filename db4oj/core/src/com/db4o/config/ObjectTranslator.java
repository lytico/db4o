/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package  com.db4o.config;

import com.db4o.*;

/**
 * translator interface to translate objects on storage and activation.
 * <br><br>
 * By writing classes that implement this interface, it is possible to
 * define how application classes are to be converted to be stored more efficiently.
 * <br><br>
 * Before starting a db4o session, translator classes need to be registered. An example:<br>
 * <code>
 * ObjectClass oc = config.objectClass("package.className");<br>
 * oc.translate(new FooTranslator());</code><br><br>
 *
 */
public interface ObjectTranslator {

    /**
	 * db4o calls this method during storage and query evaluation.
     * @param container the ObjectContainer used
     * @param applicationObject the Object to be translated
     * @return return the object to store.<br>It needs to be of the class
	 * {@link #storedClass()}.
     */
    public Object onStore(ObjectContainer container, Object applicationObject);

    /**
	 * db4o calls this method during activation.
     * @param container the ObjectContainer used
     * @param applicationObject the object to set the members on
     * @param storedObject the object that was stored
     */
    public void onActivate(ObjectContainer container, Object applicationObject, Object storedObject);

    /**
	 * return the Class you are converting to.
     * @return the Class of the object you are returning with the method
	 * {@link #onStore(ObjectContainer, Object)}
	 */
	 public Class storedClass ();
}
