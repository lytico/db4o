/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.ext;

import com.db4o.*;

/**
 * callback methods.
 * <br><br>
 * This interface only serves as a list of all available callback methods.
 * Every method is called individually, independantly of implementing this interface.<br><br>
 * <b>Using callbacks</b><br>
 * Simply implement one or more of the listed methods in your application classes to
 * do tasks before activation, deactivation, delete, new or update, to cancel the
 * action about to be performed and to respond to the performed task.
 * <br><br>Callback methods are typically used for:
 * <br>- cascaded delete
 * <br>- cascaded update
 * <br>- cascaded activation
 * <br>- restoring transient members on instantiation
 * <br><br>Callback methods follow regular calling conventions. Methods in superclasses
 * need to be called explicitely.
 * <br><br>All method calls are implemented to occur only once, upon one event.
 */
public interface ObjectCallbacks {

    /**
     * called before an Object is activated.
     * @param container the ObjectContainer the object is stored in.
     * @return false to prevent activation.
     */
    public boolean objectCanActivate(ObjectContainer container);

    /**
     * called before an Object is deactivated.
     * @param container the ObjectContainer the object is stored in.
     * @return false to prevent deactivation.
     */
    public boolean objectCanDeactivate(ObjectContainer container);

    /**
     * called before an Object is deleted.
     * <br><br>In a client/server setup this callback method will be executed on
     * the server.
     * @param container the ObjectContainer the object is stored in.
     * @return false to prevent the object from being deleted.
     */
    public boolean objectCanDelete(ObjectContainer container);

    /**
     * called before an Object is stored the first time.
     * @param container the ObjectContainer is about to be stored to.
     * @return false to prevent the object from being stored.
     */
    public boolean objectCanNew(ObjectContainer container);

    /**
     * called before a persisted Object is updated.
     * @param container the ObjectContainer the object is stored in.
     * @return false to prevent the object from being updated.
     */
    public boolean objectCanUpdate(ObjectContainer container);
    
    /**
     * called upon activation of an object.
     * @param container the ObjectContainer the object is stored in.
     */
    public void objectOnActivate(ObjectContainer container);

    /**
     * called upon deactivation of an object.
     * @param container the ObjectContainer the object is stored in.
     */
    public void objectOnDeactivate(ObjectContainer container);

    /**
     * called after an object was deleted.
     * <br><br>In a client/server setup this callback method will be executed on
     * the server.
     * @param container the ObjectContainer the object was stored in.
     */
    public void objectOnDelete(ObjectContainer container);

    /**
     * called after a new object was stored.
     * @param container the ObjectContainer the object is stored to.
     */
    public void objectOnNew(ObjectContainer container);

    /**
     * called after an object was updated.
     * @param container the ObjectContainer the object is stored in.
     */
    public void objectOnUpdate(ObjectContainer container);
}