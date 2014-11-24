/* Copyright (C) 2004 - 2005  Versant Inc.   http://www.db4o.com */

package  com.db4o;

import com.db4o.ext.*;
import com.db4o.query.Predicate;
import com.db4o.query.Query;
import com.db4o.query.QueryComparator;

import java.util.Comparator;


/**
 * The main interface to a db4o database, stand-alone or client/server.
 * <br><br>The ObjectContainer interface provides methods
 * to store, query and delete objects and to commit and rollback
 * transactions.<br><br>
 * <br><br>An ObjectContainer also represents a transaction. All work
 * with db4o always is transactional. Both {@link #commit()} and
 * {@link #rollback()} start a new transaction immediately. For working
 * against the same database with multiple transactions, open a new object container
 * with {@link #ext()}.{@link ExtObjectContainer#openSession() openSession()}
 * @see ExtObjectContainer ExtObjectContainer for extended functionality.
 * @sharpen.ignore
 */
public interface ObjectContainer {
	
    /**
     * Activates all members on a stored object to the specified depth.
     * <br><br>
     * See {@link com.db4o.config.CommonConfiguration#activationDepth(int) "Why activation"}
     * for an explanation why activation is necessary.<br><br>
     * Calling this method activates a graph of persistent objects in memory.
     * Only deactivated objects in the graph will be touched: Their
     * fields will be loaded from the database. 
     * When called it starts from the given
     * object, traverses all member objects and activates them up to the given depth.
     * The depth parameter is the distance in "field hops"
     * (object.field.field) away from the root object. The nodes at 'depth' level
     * away from the root (for a depth of 3: object.member.member) will be instantiated
     * but not populated with data. Its fields will be null.
     * The activation depth of individual classes can be overruled
     * with the methods
     * {@link com.db4o.config.ObjectClass#maximumActivationDepth maximumActivationDepth()} and
     * {@link com.db4o.config.ObjectClass#minimumActivationDepth minimumActivationDepth()} in the
     * {@link com.db4o.config.ObjectClass ObjectClass interface}.<br><br>
     * @see com.db4o.config.CommonConfiguration#activationDepth Why activation?
     * @see ObjectCallbacks Using callbacks
     * @param obj the objects to be activated.
     * @param depth the object-graph {@link com.db4o.config.CommonConfiguration#activationDepth depth} up to which object are activated
     *  @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
	 *  @throws DatabaseClosedException db4o database file was closed or failed to open.
     */
    public void activate(Object obj, int depth) throws Db4oIOException, DatabaseClosedException;
    
    /**
     * Closes the ObjectContainer.
     * <br><br>Calling close() automatically performs a
     * {@link #commit commit()}.
     * @return success - true denotes that the object container was closed, false if it was already closed
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     */
	public boolean close() throws Db4oIOException;

    /**
     * Commits the running transaction.
     * <br><br>Transactions are back-to-back. A call to commit will start
     * a new transaction immediately.
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     * @throws DatabaseReadOnlyException database was configured as read-only.
     */
    public void commit() throws Db4oIOException, DatabaseClosedException, DatabaseReadOnlyException;
    

    /**
     * Deactivates a stored object by setting all members to null.
     * <br>Primitive types will be set to their default values.
     * The method has no effect, if the passed object is not stored in the
     * object container.
     * <br><br>
     * Be aware that calling may have side effects, which assume that a object is filled with data.
     * <br><br>
     * In general you should not deactivate objects, since it makes you application more
     * complex and confusing.
     * To control the scope of objects you should use session containers
     * for your unit of work. Use {@link #ext()}{@link com.db4o.ext.ExtObjectContainer#openSession() openSession()}
     * to create a new session.
     *
	 * @see ObjectCallbacks Using callbacks
  	 * @see com.db4o.config.CommonConfiguration#activationDepth Why activation?
     * @param obj the object to be deactivated.
	 * @param depth the object-graph depth up to which object are deactivated
	 * @throws DatabaseClosedException db4o database file was closed or failed to open.
	*/
    public void deactivate(Object obj, int depth) throws DatabaseClosedException;

    /**
     * Deletes a stored object permanently from the database.
     * <br><br>Note that this method has to be called <b>for every single object
     * individually</b>. Delete does not recurs to object members. Primitives, strings
     * and array member types are deleted.
     * <br><br>Referenced objects of the passed object remain untouched, unless
     * cascaded deletes are  
     * {@link com.db4o.config.ObjectClass#cascadeOnDelete configured for the class}
     * or  {@link com.db4o.config.ObjectField#cascadeOnDelete for member fields}.
     * <br><br>The method has no effect, if
     * the passed object is not stored in the object container.
     * <br><br>A subsequent call to
     * {@link #store(Object)} with the same object stores the object again in the database.<br><br>
	 * @see com.db4o.config.ObjectClass#cascadeOnDelete
	 * @see com.db4o.config.ObjectField#cascadeOnDelete
	 * @see ObjectCallbacks Using callbacks
     * @param obj the object to be deleted from the object container
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     * @throws DatabaseReadOnlyException database was configured as read-only.
     */
    public void delete(Object obj) throws Db4oIOException, DatabaseClosedException, DatabaseReadOnlyException;
    
    /**
     * Returns an ObjectContainer with extended functionality.
     * <br><br>Every ObjectContainer that db4o provides can be casted to
     * an ExtObjectContainer. This method is supplied for your convenience
     * to work without a cast.
     * <br><br>The ObjectContainer functionality is split to two interfaces
     * to allow newcomers to focus on the essential methods.<br><br>
     * @return this, casted to ExtObjectContainer
     */
    public ExtObjectContainer ext();
	   
	/**
     * Query-By-Example interface to retrieve objects.
     * <br><br>
     * queryByExample() creates an {@link ObjectSet ObjectSet} containing
     * all objects in the database that match the passed
     * template object.<br><br>
	 * Calling queryByExample(NULL) returns all objects stored in the database.
     * <br><br>
     * <b>Query Evaluation:</b>
     * <ul><li>All non-null members of the template object are compared against
     * all stored objects of the same class.</li>
     * <li>Primitive type members are ignored if they are 0 or false respectively.</li>
     * <li>Arrays and  collections  are
     * evaluated for containment. Differences in length/size() are
     * ignored.</li>
     * </ul>
     * @param template object to be used as an example to find all matching objects.<br><br>
     * @return {@link ObjectSet ObjectSet} containing all found objects.<br><br>
	 * @see com.db4o.config.CommonConfiguration#activationDepth Why activation?
	 * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
	 * @throws DatabaseClosedException db4o database file was closed or failed to open.
	 */
    public <T> ObjectSet<T> queryByExample(Object template) throws Db4oIOException, DatabaseClosedException;
    
    /**
     * Creates a new S.O.D.A. {@link Query Query}.
     * <b>NOTE: Soda queries silently ignore invalid specified fields, constraints etc.</b>
     * <br><br>
     * {@link #query(Predicate) Native queries } are the recommended main db4o query
     * interface. 
     * <br><br>
     * @return a new Query object
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     */
    public Query query() throws DatabaseClosedException;
    
    /**
     * Queries for all instances of a class.
     * @param clazz the class to query for.
     * @return all instances of the given class
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     */
    public <TargetType> ObjectSet <TargetType> query(Class<TargetType> clazz) throws Db4oIOException, DatabaseClosedException;

    
    /**
     * Native Query Interface.<br><br>
     * <b>Make sure that you include the db4o-nqopt-java.jar and bloat.jar in your classpath
     * when using native queries. Unless you are using the db4o-all-java5.jar</b>
     * <br><br>Native Queries allows typesafe, compile-time checked and refactorable
     * queries, following object-oriented principles. A Native Query expression should return true to
     * include that object in the result and false otherwise.<br/><br/>
     * db4o will attempt to optimize native query expressions and execute them
     * against indexes and without instantiating actual objects.
     * Otherwise db4o falls back and instantiates objects to run them against the given predicate.
     * That is an order of magnitude slower than a optimized native query.<br><br>
     * 
     * <pre class="prettyprint"/><code>
     * List&lt;Cat&gt; cats = db.query(new Predicate&lt;Cat&gt;() {
     *     public boolean match(Cat cat) {
     *         return cat.getName().equals("Occam");
     *     }
     * });
     *
     * </code></pre>
     *
     * Summing up the above:<br>
     * In order to execute a Native Query, you can extend the Predicate class<br><br>
     * A class that extends Predicate is required to 
     * implement the #match() method, following the native query
     * conventions:<br>
     * - The name of the method is "#match()".<br>
     * - The method must be public.<br>
     * - The method returns a boolean.<br>
     * - The method takes one parameter.<br>
     * - The Class (Java) of the parameter specifies the extent.<br>
     * - The query expression  should return true to include a object. False otherwise.<br><br>
     *   
     * @param predicate the {@link Predicate} containing the native query expression.
     * @return the query result
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     */
    public <TargetType> ObjectSet <TargetType> query(Predicate<TargetType> predicate) throws Db4oIOException, DatabaseClosedException;

    /**
     * Native Query Interface. Queries as with {@link com.db4o.ObjectContainer#query(com.db4o.query.Predicate)},
     * but will sort the result according to the given comperator.
     * 
     * @param predicate the {@link Predicate} containing the native query expression.
     * @param comparator the {@link QueryComparator} specifying the sort order of the result
     * @return the query result
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     */
    public <TargetType> ObjectSet <TargetType> query(Predicate<TargetType> predicate,QueryComparator<TargetType> comparator) throws Db4oIOException, DatabaseClosedException;

    /**
     * Native Query Interface. Queries as with {@link com.db4o.ObjectContainer#query(com.db4o.query.Predicate)},
     * but will sort the resulting {@link com.db4o.ObjectSet} according to the given {@link Comparator}.
     * 
     * @param predicate the {@link Predicate} containing the native query expression.
     * @param comparator the java.util.Comparator specifying the sort order of the result
     * @return the {@link ObjectSet} returned by the query.
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     * @sharpen.ignore
     */
    @decaf.Ignore(decaf.Platform.JDK11)
    public <TargetType> ObjectSet <TargetType> query(Predicate<TargetType> predicate, Comparator<TargetType> comparator) throws Db4oIOException, DatabaseClosedException;

    /**
     * Rolls back the running transaction.
     * <b>This only rolls back the changes in the database, but not the state of in memory objects</b>.
     * <br><br>
     *
     * Dealing with stale state of in memory objects after a rollback:<br/>
     * <ul><li>Since in memory objects are not rolled back you probably want start with a clean state.
     * The easiest way to do this is by creating a new object container:
     * {@link #ext()}.{@link com.db4o.ext.ExtObjectContainer#openSession() openSession()}.
     * </li><li>Alternatively you can deactivate objects or {@link ExtObjectContainer#refresh(Object, int) refresh} them to get back to the state in the database.
     * </li><li>In case you are using transparent persistence you can use a {@link com.db4o.ta.RollbackStrategy rollback strategy} to rollback
     * the in memory objects as well. </li></ul>
     * @throws Db4oIOException I/O operation failed or was unexpectedly interrupted.
     * @throws DatabaseClosedException db4o database file was closed or failed to open.
     * @throws DatabaseReadOnlyException database was configured as read-only.
     */
    public void rollback() throws Db4oIOException, DatabaseClosedException, DatabaseReadOnlyException;
   

	/**
     * Stores objects or updates stored objects.
     * <br><br>An object not yet stored in the database will be
     * stored. An object already stored in database will be updated.
     * <br><br>
     * <b>Updates:</b>
     * <ul>
	 * <li>Will update all primitive types, strings and arrays of a object</li>
     * <li>References to other object that are already stored will be updated.</li>
	 * <li>New object members will be stored.</li>
     * <li>Referenced object members that are already stored are <b>not</b> updated
     * themselves. Every object member needs to be updated individually with a
	 * call to store(). Unless a deeper update depth has been configured with on of these options:
	 * {@link com.db4o.config.CommonConfiguration#updateDepth Global}- or
     * {@link com.db4o.config.ObjectClass#updateDepth class-specific update depth},
     * {@link com.db4o.config.ObjectClass#cascadeOnUpdate cascde on update for type} or
     * {@link com.db4o.config.ObjectField#cascadeOnUpdate field}.</li>
     * </ul>
     * @param obj the object to be stored or updated.
	 * @see ExtObjectContainer#store(java.lang.Object, int) ExtObjectContainer#set(object, depth)
	 * @see com.db4o.config.CommonConfiguration#updateDepth
	 * @see com.db4o.config.ObjectClass#updateDepth
	 * @see com.db4o.config.ObjectClass#cascadeOnUpdate
	 * @see com.db4o.config.ObjectField#cascadeOnUpdate
	 * @throws DatabaseClosedException db4o database file was closed or failed to open.
	 * @throws DatabaseReadOnlyException database was configured as read-only.
     */
    public void store(Object obj) throws DatabaseClosedException, DatabaseReadOnlyException;
    
    
    
}



