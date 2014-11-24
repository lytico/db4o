/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.db4o.replication.*;
import com.db4o.types.*;

/**
 * extended functionality for the
 * {@link com.db4o.ObjectContainer ObjectContainer} interface.
 * <br><br>Every db4o {@link com.db4o.ObjectContainer ObjectContainer}
 * always is an <code>ExtObjectContainer</code> so a cast is possible.<br><br>
 * {@link com.db4o.ObjectContainer#ext}
 * is a convenient method to perform the cast.<br><br>
 * The ObjectContainer functionality is split to two interfaces to allow newcomers to
 * focus on the essential methods.
 */
public interface ExtObjectContainer extends ObjectContainer {
    
    /**
     * backs up a database file of an open ObjectContainer.
     * <br><br>While the backup is running, the ObjectContainer can continue to be
     * used. Changes that are made while the backup is in progress, will be applied to
     * the open ObjectContainer and to the backup.<br><br>
     * While the backup is running, the ObjectContainer should not be closed.<br><br>
     * If a file already exists at the specified path, it will be overwritten.<br><br>
     * @param path a fully qualified path
     */
    public void backup(String path) throws IOException;


    /**
     * binds an object to an internal object ID.
     * <br><br>This method uses the ID parameter to load the 
     * correspondig stored object into memory and replaces this memory 
     * reference with the object parameter. The method may be used to replace
     * objects or to reassociate an object with it's stored instance
     * after closing and opening a database file. A subsequent call to 
     * {@link com.db4o.ObjectContainer#set set(Object)} is
     * necessary to update the stored object.<br><br>
     * <b>Requirements:</b><br>- The ID needs to be a valid internal object ID, 
     * previously retrieved with 
     * {@link #getID getID(Object)}.<br>
     * - The object parameter needs to be of the same class as the stored object.<br><br>
     * @see #getID(java.lang.Object)
     * @param obj the object that is to be bound
     * @param id the internal id the object is to be bound to
     */
    public void bind(Object obj, long id);
    
    
    /**
     * returns the {@link Db4oCollections} interface to create or modify database-aware
     * collections for this {@link ObjectContainer}.<br><br>
     * @return the {@link Db4oCollections} interface for this {@link ObjectContainer}.
     */
    public Db4oCollections collections();
    

    /**
     * returns the Configuration context for this ObjectContainer.
     * <br><br>
     * Upon opening an ObjectContainer with any of the factory methods in the
     * {@link com.db4o.Db4o Db4o class}, the global 
     * {@link com.db4o.config.Configuration Configuration} context
     * is copied into the ObjectContainer. The 
     * {@link com.db4o.config.Configuration Configuration}
     * can be modified individually for
     * each ObjectContainer without any effects on the global settings.<br><br>
     * @return {@link com.db4o.config.Configuration Configuration} the Configuration
     * context for this ObjectContainer
     * @see Db4o#configure
     */
    public Configuration configure();

    /**
     * returns the stored object for an internal ID.
     * <br><br>This is the fastest method for direct access to objects. Internal
     * IDs can be obtained with {@link #getID getID(Object)}.
     * Objects will not be activated by this method. They will be returned in the 
     * activation state they are currently in, in the local cache.<br><br>
     * @param ID the internal ID
     * @return the object associated with the passed ID or <code>null</code>, 
     * if no object is associated with this ID in this <code>ObjectContainer</code>.
     * @see com.db4o.config.Configuration#activationDepth Why activation?
     */
    public Object getByID(long ID);
    
    /**
     * returns a stored object for a {@link Db4oUUID}.
     * <br><br>
     * This method is intended for replication and for long-term
     * external references to objects. To get a {@link Db4oUUID} for an
     * object use {@link #getObjectInfo(Object)} and {@link ObjectInfo#getUUID()}.<br><br> 
     * Objects will not be activated by this method. They will be returned in the 
     * activation state they are currently in, in the local cache.<br><br>
     * @param uuid the UUID
     * @return the object for the UUID
     * @see com.db4o.config.Configuration#activationDepth Why activation?
     */
    public Object getByUUID(Db4oUUID uuid);

    /**
     * returns the internal unique object ID.
     * <br><br>db4o assigns an internal ID to every object that is stored. IDs are 
     * guaranteed to be unique within one <code>ObjectContainer</code>. 
     * An object carries the same ID in every db4o session. Internal IDs can 
     * be used to look up objects with the very fast 
     * {@link #getByID getByID} method.<br><br>
     * Internal IDs will change when a database is defragmented. Use 
     * {@link #getObjectInfo(Object)}, {@link ObjectInfo#getUUID()} and
     * {@link #getByUUID(Db4oUUID)} for long-term external references to
     * objects.<br><br>  
     * @param obj any object
     * @return the associated internal ID or <code>0</code>, if the passed
     * object is not stored in this <code>ObjectContainer</code>.
     */
    public long getID(Object obj);
    
    /**
     * returns the {@link ObjectInfo} for a stored object.
     * <br><br>This method will return null, if the passed
     * object is not stored to this <code>ObjectContainer</code>.<br><br>
     * @param obj the stored object 
     * @return the {@link ObjectInfo} 
     */
    public ObjectInfo getObjectInfo(Object obj);
    
    /**
     * returns the Db4oDatabase identity object for this ObjectContainer. 
     * @return the Db4oDatabase identity object for this ObjectContainer.
     */
    public Db4oDatabase identity();

    /**
     * tests if an object is activated.
     * <br><br><code>isActive</code> returns <code>false</code> if an object is not
     * stored within the <code>ObjectContainer</code>.<br><br>
     * @param obj to be tested<br><br>
     * @return <code>true</code> if the passed object is active.
     */
    public boolean isActive(Object obj);

    /**
     * tests if an object with this ID is currently cached.
     * <br><br>
     * @param ID the internal ID
     */
    public boolean isCached(long ID);

    /**
     * tests if this <code>ObjectContainer</code> is closed.
     * <br><br>
     * @return <code>true</code> if this <code>ObjectContainer</code> is closed.
     */
    public boolean isClosed();

    /**
     * tests if an object is stored in this <code>ObjectContainer</code>.
     * <br><br>
     * @param obj to be tested<br><br>
     * @return <code>true</code> if the passed object is stored.
     */
    public boolean isStored(Object obj);
    
    /**
     * returns all class representations that are known to this
     * ObjectContainer because they have been used or stored.
     * @return all class representations that are known to this
     * ObjectContainer because they have been used or stored. 
     */
    public ReflectClass[] knownClasses();

    /**
     * returns the main synchronisation lock.
     * <br><br>
     * Synchronize over this object to ensure exclusive access to
     * the ObjectContainer.<br><br> 
     * Handle the use of this functionality with extreme care,
     * since deadlocks can be produced with just two lines of code.
     * @return Object the ObjectContainer lock object
     */
    public Object lock();
    
    
    /**
     * aids migration of objects between ObjectContainers.  
     * <br><br>When objects are migrated from one ObjectContainer to another, it is
     * desirable to preserve virtual object attributes such as the object version number
     * or the UUID. Use this method to signal to an ObjectContainer that it should read
     * existing version numbers and UUIDs from another ObjectContainer. This method should
     * also be used during the Defragment. It is included in the default
     * implementation supplied in Defragment.java/Defragment.cs.<br><br>
     * @param objectContainer the {@link ObjectContainer} objects are to be migrated
     * from or <code>null</code> to denote that migration is completed.
     */
    public void migrateFrom(ObjectContainer objectContainer);
    
	/**
	 * returns a transient copy of a persistent object with all members set
	 * to the values that are currently stored to the database.  
	 * <br><br>
	 * The returned objects have no connection to the database.<br><br> 
	 * With the <code>committed</code> parameter it is possible to specify,
	 * whether the desired object should contain the committed values or the
	 * values that were set by the running transaction with 
	 * {@link ObjectContainer#set(java.lang.Object)}.
	 * <br><br>A possible usecase for this feature:<br>
	 * An application might want to check all changes applied to an object
	 * by the running transaction.<br><br>
	 * @param object the object that is to be cloned
	 * @param depth the member depth to which the object is to be instantiated  
	 * @param committed whether committed or set values are to be returned
	 * @return the object
	 */
    public Object peekPersisted(Object object,int depth, boolean committed);
    

    /**
    * unloads all clean indices from memory and frees unused objects.
    * <br><br>Call commit() and purge() consecutively to achieve the best
    * result possible. This method can have a negative impact 
    * on performance since indices will have to be reread before further 
    * inserts, updates or queries can take place.
    */
    public void purge();

    /**
     * unloads a specific object from the db4o reference mechanism.
     * <br><br>db4o keeps references to all newly stored and 
     * instantiated objects in memory, to be able to manage object identities. 
     * <br><br>With calls to this method it is possible to remove an object from the
     * reference mechanism, to allow it to be garbage collected. You are not required to
     * call this method in the .NET and JDK 1.2 versions, since objects are
     * referred to by weak references and garbage collection happens
     * automatically.<br><br>An object removed with  <code>purge(Object)</code> is not
     * "known" to the <code>ObjectContainer</code> afterwards, so this method may also be
     * used to create multiple copies of  objects.<br><br> <code>purge(Object)</code> has
     * no influence on the persistence state of objects. "Purged" objects can be
     * reretrieved with queries.<br><br>
     * @param obj the object to be removed from the reference mechanism.
     */
    public void purge(Object obj);
    
	/**
	 * Return the reflector currently being used by db4objects.
	 * 
	 * @return the current Reflector.
	 */
	public GenericReflector reflector();
	
    /**
     * refreshs all members on a stored object to the specified depth.
     * <br><br>If a member object is not activated, it will be activated by this method.
     * <br><br>The isolation used is READ COMMITTED. This method will read all objects
     * and values that have been committed by other transactions.<br><br>
     * @param obj the object to be refreshed.
	 * @param depth the member {@link Configuration#activationDepth(int) depth}
	 *  to which refresh is to cascade.
     */
    public void refresh(Object obj, int depth);


    /**
     * releases a semaphore, if the calling transaction is the owner.
     * @param name the name of the semaphore to be released.
     */
    public void releaseSemaphore(String name);
    
    /**
     * prepares for replication with another {@link ObjectContainer}.
     * <br><br>An {@link ObjectContainer} can only be involved in a replication 
     * process with one other {@link ObjectContainer} at the same time.<br><br>
     * The returned {@link ReplicationProcess} interface provides methods to commit
     * and to cancel the replication process.
     * <br><br>This ObjectContainer will be "peerA" for the
     * returned ReplicationProcess. The other ObjectContainer will be "peerB".
     * @param peerB the {@link ObjectContainer} to replicate with.
     * @param conflictHandler the conflict handler for this ReplicationProcess. 
     * Conflicts occur
     * whenever {@link ReplicationProcess#replicate(Object)} is called with an 
     * object that was modified in both ObjectContainers since the last 
     * replication run between the two. Upon a conflict the 
     * {@link ReplicationConflictHandler#resolveConflict(ReplicationProcess, Object, Object)}
     * method will be called in the conflict handler.
     * @return the {@link ReplicationProcess} interface for this replication process.
     */
    public ReplicationProcess replicationBegin(ObjectContainer peerB, ReplicationConflictHandler conflictHandler);
    
    
    /**
     * deep update interface to store or update objects.
     * <br><br>In addition to the normal storage interface, 
     * {@link com.db4o.ObjectContainer#set ObjectContainer#set(Object)},
     * this method allows a manual specification of the depth, the passed object is to be updated.<br><br>
     * @param obj the object to be stored or updated.
     * @param depth the depth to which the object is to be updated
     * @see com.db4o.ObjectContainer#set
     */
    public void set (Object obj, int depth);
    

    /**
     * attempts to set a semaphore.
     * <br><br>
     * Semaphores are transient multi-purpose named flags for
     * {@link ObjectContainer ObjectContainers}.
     * <br><br>
     * A transaction that successfully sets a semaphore becomes
     * the owner of the semaphore. Semaphores can only be owned
     * by a single transaction at one point in time.<br><br>
     * This method returns true, if the transaction already owned
     * the semaphore before the method call or if it successfully
     * acquires ownership of the semaphore.<br><br>
     * The waitForAvailability parameter allows to specify a time
     * in milliseconds to wait for other transactions to release
     * the semaphore, in case the semaphore is already owned by
     * another transaction.<br><br>
     * Semaphores are released by the first occurence of one of the
     * following:<br>
     * - the transaction releases the semaphore with 
     * {@link #releaseSemaphore(java.lang.String)}<br> - the transaction is closed with {@link
     * ObjectContainer#close()}<br> - C/S only: the corresponding {@link ObjectServer} is
     * closed.<br> - C/S only: the client {@link ObjectContainer} looses the connection and is timed
     * out.<br><br> Semaphores are set immediately. They are independant of calling {@link
     * ObjectContainer#commit()} or {@link ObjectContainer#rollback()}.<br><br> <b>Possible usecases
     * for semaphores:</b><br> - prevent other clients from inserting a singleton at the same time.
     * A suggested name for the semaphore:  "SINGLETON_" + Object#getClass().getName().<br>  - lock
     * objects. A suggested name:   "LOCK_" + {@link #getID(java.lang.Object) getID(Object)}<br> -
     * generate a unique client ID. A suggested name:  "CLIENT_" +
     * System.currentTimeMillis().<br><br>   
     * 
     * @param name the name of the semaphore to be set
     * @param waitForAvailability the time in milliseconds to wait for other
     * transactions to release the semaphore. The parameter may be zero, if
     * the method is to return immediately. 
     * @return boolean flag 
     * <br><code>true</code>, if the semaphore could be set or if the 
     * calling transaction already owned the semaphore.
     * <br><code>false</code>, if the semaphore is owned by another
     * transaction.
     */
    public boolean setSemaphore(String name, int waitForAvailability);
    
    /**
	* returns a {@link StoredClass} meta information object.
	* <br><br>
	* There are three options how to use this method.<br>
	* Any of the following parameters are possible:<br>
	* - a fully qualified classname.<br>
	* - a Class object.<br>
	* - any object to be used as a template.<br><br>
	* @param clazz class name, Class object, or example object.<br><br>
	* @return an instance of an {@link StoredClass} meta information object.
	*/
    public StoredClass storedClass(Object clazz);

    /**
     * returns an array of all {@link StoredClass} meta information objects.
     */
    public StoredClass[] storedClasses();
    
    /**
     * returns the current transaction serial number.
     * <br><br>This serial number can be used to query for modified objects
     * and for replication purposes.
     * @return the current transaction serial number.
     */
    public long version();
	
}