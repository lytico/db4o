/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.types;

/**
 * factory and other methods for database-aware collections.
 */
public interface Db4oCollections {
    
    /**
     * creates a new database-aware linked list.
     * <br><br>Usage:<br>
     * - declare a <code>java.util.List</code> variable in your persistent class.<br>
     * - fill this variable with this method.<br><br>
     * <b>Example:</b><br><br>
     * <code><pre>
     * class MyClass{
     *     List myList;
     * }
     * 
     * MyClass myObject = new MyClass(); 
     * myObject.myList = objectContainer.ext().collections().newLinkedList();</pre></code><br><br>
     * @return {@link Db4oList}
     * @see Db4oList
     */
    public Db4oList newLinkedList();
    
    
    /**
     * creates a new database-aware HashMap.
     * <br><br>
     * This map will call the hashCode() method on the key objects to calculate the
     * hash value. Since the hash value is stored to the ObjectContainer, key objects
     * will have to return the same hashCode() value in every VM session.  
     * <br><br>
     * Usage:<br>
     * - declare a <code>java.util.Map</code> variable in your persistent class.<br>
     * - fill the variable with this method.<br><br>
     * <b>Example:</b><br><br>
     * <code><pre>
     * class MyClass{
     *     Map myMap;
     * } 
     * 
     * MyClass myObject = new MyClass(); 
     * myObject.myMap = objectContainer.ext().collections().newHashMap(0);</pre></code><br><br>
     * @param initialSize the initial size of the HashMap
     * @return {@link Db4oMap}
     * @see Db4oMap
     */
    public Db4oMap newHashMap(int initialSize);
    
    
    /**
     * creates a new database-aware IdentityHashMap.
     * <br><br>
     * Only first class objects already stored to the ObjectContainer (Objects with a db4o ID) 
     * can be used as keys for this type of Map. The internal db4o ID will be used as
     * the hash value.
     * <br><br>
     * Usage:<br>
     * - declare a <code>java.util.Map</code> variable in your persistent class.<br>
     * - fill the variable with this method.<br><br>
     * <b>Example:</b><br><br>
     * <code><pre>
     * class MyClass{
     *     Map myMap;
     * }
     * 
     * MyClass myObject = new MyClass(); 
     * myObject.myMap = objectContainer.ext().collections().newIdentityMap(0);</pre></code><br><br>
     * @param initialSize the initial size of the HashMap
     * @return {@link Db4oMap}
     * @see Db4oMap
     */
    public Db4oMap newIdentityHashMap(int initialSize);
    
    

}
