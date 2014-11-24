/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// extended functionality for the
	/// <see cref="Db4objects.Db4o.IObjectContainer">IObjectContainer</see>
	/// interface.
	/// <br /><br />Every db4o
	/// <see cref="Db4objects.Db4o.IObjectContainer">IObjectContainer</see>
	/// always is an ExtObjectContainer so a cast is possible.<br /><br />
	/// <see cref="Db4objects.Db4o.IObjectContainer.Ext()">ObjectContainer.ext()</see>
	/// is a convenient method to perform the cast.<br /><br />
	/// The ObjectContainer functionality is split to two interfaces to allow newcomers to
	/// focus on the essential methods.
	/// </summary>
	public partial interface IExtObjectContainer : IObjectContainer
	{
		/// <summary>activates an object with the current activation strategy.</summary>
		/// <remarks>
		/// activates an object with the current activation strategy.
		/// In regular activation mode the object will be activated to the
		/// global activation depth, ( see
		/// <see cref="Db4objects.Db4o.Config.ICommonConfiguration.ActivationDepth()">Db4objects.Db4o.Config.ICommonConfiguration.ActivationDepth()
		/// 	</see>
		/// )
		/// and all configured settings for
		/// <see cref="Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth(int)">Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth(int)
		/// 	</see>
		/// 
		/// and
		/// <see cref="Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth(int)">Db4objects.Db4o.Config.IObjectClass.MaximumActivationDepth(int)
		/// 	</see>
		/// will be respected.<br /><br />
		/// In Transparent Activation Mode ( see
		/// <see cref="Db4objects.Db4o.TA.TransparentActivationSupport">Db4objects.Db4o.TA.TransparentActivationSupport
		/// 	</see>
		/// )
		/// the parameter object will only be activated, if it does not implement
		/// <see cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable</see>
		/// . All referenced members that do not implement
		/// <see cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable</see>
		/// will also be activated. Any
		/// <see cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable</see>
		/// objects
		/// along the referenced graph will break cascading activation.
		/// </remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		void Activate(object obj);

		/// <summary>deactivates an object.</summary>
		/// <remarks>
		/// deactivates an object.
		/// Only the passed object will be deactivated, i.e, no object referenced by this
		/// object will be deactivated.
		/// </remarks>
		/// <param name="obj">the object to be deactivated.</param>
		void Deactivate(object obj);

		/// <summary>backs up a database file of an open ObjectContainer.</summary>
		/// <remarks>
		/// backs up a database file of an open ObjectContainer.
		/// <br /><br />While the backup is running, the ObjectContainer can continue to be
		/// used. Changes that are made while the backup is in progress, will be applied to
		/// the open ObjectContainer and to the backup.<br /><br />
		/// While the backup is running, the ObjectContainer should not be closed.<br /><br />
		/// If a file already exists at the specified path, it will be overwritten.<br /><br />
		/// The
		/// <see cref="Db4objects.Db4o.IO.IStorage">Db4objects.Db4o.IO.IStorage</see>
		/// used for backup is the one configured for this container.
		/// </remarks>
		/// <param name="path">a fully qualified path</param>
		/// <exception cref="DatabaseClosedException">db4o database file was closed or failed to open.
		/// 	</exception>
		/// <exception cref="System.NotSupportedException">
		/// is thrown when the operation is not supported in current
		/// configuration/environment
		/// </exception>
		/// <exception cref="Db4oIOException">I/O operation failed or was unexpectedly interrupted.
		/// 	</exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		void Backup(string path);

		/// <summary>backs up a database file of an open ObjectContainer.</summary>
		/// <remarks>
		/// backs up a database file of an open ObjectContainer.
		/// <br /><br />While the backup is running, the ObjectContainer can continue to be
		/// used. Changes that are made while the backup is in progress, will be applied to
		/// the open ObjectContainer and to the backup.<br /><br />
		/// While the backup is running, the ObjectContainer should not be closed.<br /><br />
		/// If a file already exists at the specified path, it will be overwritten.<br /><br />
		/// This method is intended for cross-storage backups, i.e. backup from an in-memory
		/// database to a file.
		/// </remarks>
		/// <param name="targetStorage">
		/// the
		/// <see cref="Db4objects.Db4o.IO.IStorage">Db4objects.Db4o.IO.IStorage</see>
		/// to be used for backup
		/// </param>
		/// <param name="path">a fully qualified path</param>
		/// <exception cref="DatabaseClosedException">db4o database file was closed or failed to open.
		/// 	</exception>
		/// <exception cref="System.NotSupportedException">
		/// is thrown when the operation is not supported in current
		/// configuration/environment
		/// </exception>
		/// <exception cref="Db4oIOException">I/O operation failed or was unexpectedly interrupted.
		/// 	</exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		void Backup(IStorage targetStorage, string path);

		/// <summary>binds an object to an internal object ID.</summary>
		/// <remarks>
		/// binds an object to an internal object ID.
		/// <br /><br />This method uses the ID parameter to load the
		/// corresponding stored object into memory and replaces this memory
		/// reference with the object parameter. The method may be used to replace
		/// objects or to reassociate an object with it's stored instance
		/// after closing and opening a database file. A subsequent call to
		/// <see cref="Db4objects.Db4o.IObjectContainer.Store(object)">store(Object)</see>
		/// is
		/// necessary to update the stored object.<br /><br />
		/// <b>Requirements:</b><br />- The ID needs to be a valid internal object ID,
		/// previously retrieved with
		/// <see cref="GetID(object)">getID(Object)</see>
		/// .<br />
		/// - The object parameter needs to be of the same class as the stored object.<br /><br />
		/// </remarks>
		/// <seealso cref="GetID(object)">GetID(object)</seealso>
		/// <param name="obj">the object that is to be bound</param>
		/// <param name="id">the internal id the object is to be bound to</param>
		/// <exception cref="DatabaseClosedException">db4o database file was closed or failed to open.
		/// 	</exception>
		/// <exception cref="InvalidIDException">
		/// when the provided id is outside the scope of the
		/// database IDs.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidIDException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		void Bind(object obj, long id);

		/// <summary>returns the Configuration context for this ObjectContainer.</summary>
		/// <remarks>
		/// returns the Configuration context for this ObjectContainer.
		/// <br />
		/// </remarks>
		/// <returns>
		/// 
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// the Configuration
		/// context for this ObjectContainer
		/// </returns>
		IConfiguration Configure();

		/// <summary>returns a member at the specific path without activating intermediate objects.
		/// 	</summary>
		/// <remarks>
		/// returns a member at the specific path without activating intermediate objects.
		/// <br /><br />
		/// This method allows navigating from a persistent object to it's members in a
		/// performant way without activating or instantiating intermediate objects.
		/// </remarks>
		/// <param name="obj">the parent object that is to be used as the starting point.</param>
		/// <param name="path">an array of field names to navigate by</param>
		/// <returns>the object at the specified path or null if no object is found</returns>
		object Descend(object obj, string[] path);

		/// <summary>returns the stored object for an internal ID.</summary>
		/// <remarks>
		/// returns the stored object for an internal ID.
		/// <br /><br />This is the fastest method for direct access to objects. Internal
		/// IDs can be obtained with
		/// <see cref="GetID(object)">getID(Object)</see>
		/// .
		/// Objects will not be activated by this method. They will be returned in the
		/// activation state they are currently in, in the local cache.<br /><br />
		/// </remarks>
		/// <param name="Id">the internal ID</param>
		/// <returns>
		/// the object associated with the passed ID or null,
		/// if no object is associated with this ID in this ObjectContainer.
		/// </returns>
		/// <seealso cref="Db4objects.Db4o.Config.ICommonConfiguration.ActivationDepth()">Why activation?
		/// 	</seealso>
		/// <exception cref="DatabaseClosedException">db4o database file was closed or failed to open.
		/// 	</exception>
		/// <exception cref="InvalidIDException">when an invalid id is passed</exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidIDException"></exception>
		object GetByID(long Id);

		/// <summary>
		/// returns a stored object for a
		/// <see cref="Db4oUUID">Db4oUUID</see>
		/// .
		/// <br /><br />
		/// This method is intended for replication and for long-term
		/// external references to objects. To get a
		/// <see cref="Db4oUUID">Db4oUUID</see>
		/// for an
		/// object use
		/// <see cref="GetObjectInfo(object)">GetObjectInfo(object)</see>
		/// and
		/// <see cref="IObjectInfo.GetUUID()">IObjectInfo.GetUUID()</see>
		/// .<br /><br />
		/// Objects will not be activated by this method. They will be returned in the
		/// activation state they are currently in, in the local cache.<br /><br />
		/// </summary>
		/// <param name="uuid">the UUID</param>
		/// <returns>the object for the UUID</returns>
		/// <seealso cref="Db4objects.Db4o.Config.ICommonConfiguration.ActivationDepth()">Why activation?
		/// 	</seealso>
		/// <exception cref="Db4oIOException">I/O operation failed or was unexpectedly interrupted.
		/// 	</exception>
		/// <exception cref="DatabaseClosedException">db4o database file was closed or failed to open.
		/// 	</exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		object GetByUUID(Db4oUUID uuid);

		/// <summary>returns the internal unique object ID.</summary>
		/// <remarks>
		/// returns the internal unique object ID.
		/// <br /><br />db4o assigns an internal ID to every object that is stored. IDs are
		/// guaranteed to be unique within one ObjectContainer.
		/// An object carries the same ID in every db4o session. Internal IDs can
		/// be used to look up objects with the very fast
		/// <see cref="GetByID(long)">getByID</see>
		/// method.<br /><br />
		/// Internal IDs will change when a database is defragmented. Use
		/// <see cref="GetObjectInfo(object)">GetObjectInfo(object)</see>
		/// ,
		/// <see cref="IObjectInfo.GetUUID()">IObjectInfo.GetUUID()</see>
		/// and
		/// <see cref="GetByUUID(Db4oUUID)">GetByUUID(Db4oUUID)</see>
		/// for long-term external references to
		/// objects.<br /><br />
		/// </remarks>
		/// <param name="obj">any object</param>
		/// <returns>
		/// the associated internal ID or <code>0</code>, if the passed
		/// object is not stored in this ObjectContainer.
		/// </returns>
		long GetID(object obj);

		/// <summary>
		/// returns the
		/// <see cref="IObjectInfo">IObjectInfo</see>
		/// for a stored object.
		/// <br /><br />This method will return null, if the passed
		/// object is not stored to this ObjectContainer.<br /><br />
		/// </summary>
		/// <param name="obj">the stored object</param>
		/// <returns>
		/// the
		/// <see cref="IObjectInfo">IObjectInfo</see>
		/// 
		/// </returns>
		IObjectInfo GetObjectInfo(object obj);

		/// <summary>returns the Db4oDatabase object for this ObjectContainer.</summary>
		/// <remarks>returns the Db4oDatabase object for this ObjectContainer.</remarks>
		/// <returns>the Db4oDatabase identity object for this ObjectContainer.</returns>
		Db4oDatabase Identity();

		/// <summary>tests if an object is activated.</summary>
		/// <remarks>
		/// tests if an object is activated.
		/// <br /><br />isActive returns false if an object is not
		/// stored within the ObjectContainer.<br /><br />
		/// </remarks>
		/// <param name="obj">to be tested<br /><br /></param>
		/// <returns>true if the passed object is active.</returns>
		bool IsActive(object obj);

		/// <summary>tests if an object with this ID is currently cached.</summary>
		/// <remarks>
		/// tests if an object with this ID is currently cached.
		/// <br /><br />
		/// </remarks>
		/// <param name="Id">the internal ID</param>
		bool IsCached(long Id);

		/// <summary>tests if this ObjectContainer is closed.</summary>
		/// <remarks>
		/// tests if this ObjectContainer is closed.
		/// <br /><br />
		/// </remarks>
		/// <returns>true if this ObjectContainer is closed.</returns>
		bool IsClosed();

		/// <summary>tests if an object is stored in this ObjectContainer.</summary>
		/// <remarks>
		/// tests if an object is stored in this ObjectContainer.
		/// <br /><br />
		/// </remarks>
		/// <param name="obj">to be tested<br /><br /></param>
		/// <returns>true if the passed object is stored.</returns>
		/// <exception cref="DatabaseClosedException">db4o database file was closed or failed to open.
		/// 	</exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		bool IsStored(object obj);

		/// <summary>
		/// returns all class representations that are known to this
		/// ObjectContainer because they have been used or stored.
		/// </summary>
		/// <remarks>
		/// returns all class representations that are known to this
		/// ObjectContainer because they have been used or stored.
		/// </remarks>
		/// <returns>
		/// all class representations that are known to this
		/// ObjectContainer because they have been used or stored.
		/// </returns>
		IReflectClass[] KnownClasses();

		/// <summary>returns the main synchronization lock.</summary>
		/// <remarks>
		/// returns the main synchronization lock.
		/// <br /><br />
		/// Synchronize over this object to ensure exclusive access to
		/// the ObjectContainer.<br /><br />
		/// Handle the use of this functionality with extreme care,
		/// since deadlocks can be produced with just two lines of code.
		/// </remarks>
		/// <returns>Object the ObjectContainer lock object</returns>
		object Lock();

		/// <summary>opens a new ObjectContainer on top of this ObjectContainer.</summary>
		/// <remarks>
		/// opens a new ObjectContainer on top of this ObjectContainer.
		/// The ObjectContainer will have it's own transaction and
		/// it's own reference system.
		/// </remarks>
		/// <returns>the new ObjectContainer session.</returns>
		/// <since>8.0</since>
		IObjectContainer OpenSession();

		/// <summary>
		/// returns a transient copy of a persistent object with all members set
		/// to the values that are currently stored to the database.
		/// </summary>
		/// <remarks>
		/// returns a transient copy of a persistent object with all members set
		/// to the values that are currently stored to the database.
		/// <br /><br />
		/// The returned objects have no connection to the database.<br /><br />
		/// With the committed parameter it is possible to specify,
		/// whether the desired object should contain the committed values or the
		/// values that were set by the running transaction with
		/// <see cref="Db4objects.Db4o.IObjectContainer.Store(object)">Db4objects.Db4o.IObjectContainer.Store(object)
		/// 	</see>
		/// .
		/// <br /><br />A possible use case for this feature:<br />
		/// An application might want to check all changes applied to an object
		/// by the running transaction.<br /><br />
		/// </remarks>
		/// <param name="object">the object that is to be cloned</param>
		/// <param name="depth">the member depth to which the object is to be instantiated</param>
		/// <param name="committed">whether committed or set values are to be returned</param>
		/// <returns>the object</returns>
		object PeekPersisted(object @object, int depth, bool committed);

		/// <summary>unloads all clean indices from memory and frees unused objects.</summary>
		/// <remarks>
		/// unloads all clean indices from memory and frees unused objects.
		/// <br /><br /> This method can have a negative impact
		/// on performance since indices will have to be reread before further
		/// inserts, updates or queries can take place.
		/// </remarks>
		void Purge();

		/// <summary>unloads a specific object from the db4o reference mechanism.</summary>
		/// <remarks>
		/// unloads a specific object from the db4o reference mechanism.
		/// <br /><br />db4o keeps references to all newly stored and
		/// instantiated objects in memory, to be able to manage object identities.
		/// <br /><br />With calls to this method it is possible to remove an object from the
		/// reference mechanism.<br />An object removed with  purge(Object) is not
		/// "known" to the ObjectContainer afterwards, so this method may also be
		/// used to create multiple copies of  objects.<br /><br /> purge(Object) has
		/// no influence on the persistence state of objects. "Purged" objects can be
		/// reretrieved with queries.<br /><br />
		/// </remarks>
		/// <param name="obj">the object to be removed from the reference mechanism.</param>
		void Purge(object obj);

		/// <summary>Return the reflector currently being used by db4objects.</summary>
		/// <remarks>Return the reflector currently being used by db4objects.</remarks>
		/// <returns>the current Reflector.</returns>
		GenericReflector Reflector();

		/// <summary>refreshs all members on a stored object to the specified depth.</summary>
		/// <remarks>
		/// refreshs all members on a stored object to the specified depth.
		/// <br /><br />If a member object is not activated, it will be activated by this method.
		/// <br /><br />The isolation used is READ COMMITTED. This method will read all objects
		/// and values that have been committed by other transactions.<br /><br />
		/// </remarks>
		/// <param name="obj">the object to be refreshed.</param>
		/// <param name="depth">
		/// the member
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.ActivationDepth(int)">depth</see>
		/// to which refresh is to cascade.
		/// </param>
		void Refresh(object obj, int depth);

		/// <summary>releases a semaphore, if the calling transaction is the owner.</summary>
		/// <remarks>releases a semaphore, if the calling transaction is the owner.</remarks>
		/// <param name="name">the name of the semaphore to be released.</param>
		void ReleaseSemaphore(string name);

		/// <summary>deep update interface to store or update objects.</summary>
		/// <remarks>
		/// deep update interface to store or update objects.
		/// <br /><br />In addition to the normal storage interface,
		/// <see cref="Db4objects.Db4o.IObjectContainer.Store(object)">ObjectContainer#store(Object)
		/// 	</see>
		/// ,
		/// this method allows a manual specification of the depth, the passed object is to be updated.<br /><br />
		/// </remarks>
		/// <param name="obj">the object to be stored or updated.</param>
		/// <param name="depth">the depth to which the object is to be updated</param>
		/// <seealso cref="Db4objects.Db4o.IObjectContainer.Store(object)">Db4objects.Db4o.IObjectContainer.Store(object)
		/// 	</seealso>
		void Store(object obj, int depth);

		/// <summary>attempts to set a semaphore.</summary>
		/// <remarks>
		/// attempts to set a semaphore.
		/// <br /><br />
		/// Semaphores are transient multi-purpose named flags for
		/// <see cref="Db4objects.Db4o.IObjectContainer">ObjectContainers</see>
		/// .
		/// <br /><br />
		/// A transaction that successfully sets a semaphore becomes
		/// the owner of the semaphore. Semaphores can only be owned
		/// by a single transaction at one point in time.<br /><br />
		/// This method returns true, if the transaction already owned
		/// the semaphore before the method call or if it successfully
		/// acquires ownership of the semaphore.<br /><br />
		/// The waitForAvailability parameter allows to specify a time
		/// in milliseconds to wait for other transactions to release
		/// the semaphore, in case the semaphore is already owned by
		/// another transaction.<br /><br />
		/// Semaphores are released by the first occurrence of one of the
		/// following:<br />
		/// - the transaction releases the semaphore with
		/// <see cref="ReleaseSemaphore(string)">ReleaseSemaphore(string)</see>
		/// <br /> - the transaction is closed with
		/// <see cref="Db4objects.Db4o.IObjectContainer.Close()">Db4objects.Db4o.IObjectContainer.Close()
		/// 	</see>
		/// <br /> - C/S only: the corresponding
		/// <see cref="Db4objects.Db4o.IObjectServer">Db4objects.Db4o.IObjectServer</see>
		/// is
		/// closed.<br /> - C/S only: the client
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// looses the connection and is timed
		/// out.<br /><br /> Semaphores are set immediately. They are independant of calling
		/// <see cref="Db4objects.Db4o.IObjectContainer.Commit()">Db4objects.Db4o.IObjectContainer.Commit()
		/// 	</see>
		/// or
		/// <see cref="Db4objects.Db4o.IObjectContainer.Rollback()">Db4objects.Db4o.IObjectContainer.Rollback()
		/// 	</see>
		/// .<br /><br /> <b>Possible use cases
		/// for semaphores:</b><br /> - prevent other clients from inserting a singleton at the same time.
		/// A suggested name for the semaphore:  "SINGLETON_" + Object#getClass().getName().<br />  - lock
		/// objects. A suggested name:   "LOCK_" +
		/// <see cref="GetID(object)">getID(Object)</see>
		/// <br /> -
		/// generate a unique client ID. A suggested name:  "CLIENT_" +
		/// System.currentTimeMillis().<br /><br />
		/// </remarks>
		/// <param name="name">the name of the semaphore to be set</param>
		/// <param name="waitForAvailability">
		/// the time in milliseconds to wait for other
		/// transactions to release the semaphore. The parameter may be zero, if
		/// the method is to return immediately.
		/// </param>
		/// <returns>
		/// boolean flag
		/// <br />true, if the semaphore could be set or if the
		/// calling transaction already owned the semaphore.
		/// <br />false, if the semaphore is owned by another
		/// transaction.
		/// </returns>
		bool SetSemaphore(string name, int waitForAvailability);

		/// <summary>
		/// returns a
		/// <see cref="IStoredClass">IStoredClass</see>
		/// meta information object.
		/// <br /><br />
		/// There are three options how to use this method.<br />
		/// Any of the following parameters are possible:<br />
		/// - a fully qualified class name.<br />
		/// - a Class object.<br />
		/// - any object to be used as a template.<br /><br />
		/// </summary>
		/// <param name="clazz">class name, Class object, or example object.<br /><br /></param>
		/// <returns>
		/// an instance of an
		/// <see cref="IStoredClass">IStoredClass</see>
		/// meta information object.
		/// </returns>
		IStoredClass StoredClass(object clazz);

		/// <summary>
		/// returns an array of all
		/// <see cref="IStoredClass">IStoredClass</see>
		/// meta information objects.
		/// </summary>
		IStoredClass[] StoredClasses();

		/// <summary>
		/// returns the
		/// <see cref="ISystemInfo">ISystemInfo</see>
		/// for this ObjectContainer.
		/// <br /><br />The
		/// <see cref="ISystemInfo">ISystemInfo</see>
		/// supplies methods that provide
		/// information about system state and system settings of this
		/// ObjectContainer.
		/// </summary>
		/// <returns>
		/// the
		/// <see cref="ISystemInfo">ISystemInfo</see>
		/// for this ObjectContainer.
		/// </returns>
		ISystemInfo SystemInfo();

		/// <summary>returns the current transaction serial number.</summary>
		/// <remarks>
		/// returns the current transaction serial number.
		/// <br /><br />This serial number can be used to query for modified objects
		/// and for replication purposes.
		/// </remarks>
		/// <returns>the current transaction serial number.</returns>
		long Version();
	}
}
