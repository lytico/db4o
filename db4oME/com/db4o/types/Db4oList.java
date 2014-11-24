/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.types;

/**
 *  db4o List implementation for database-aware lists.
 * <br><br>
 * A <code>Db4oList</code> supplies the methods specified in java.util.List.<br><br>
 * All access to the list is controlled by the {@link com.db4o.ObjectContainer ObjectContainer} to help the
 * programmer produce expected results with as little work as possible:<br>  
 * - newly added objects are automatically persisted.<br>
 * - list elements are automatically activated when they are needed. The activation
 * depth is configurable with {@link Db4oCollection#activationDepth(int)}.<br>
 * - removed objects can be deleted automatically, if the list is configured
 * with {@link Db4oCollection#deleteRemoved(boolean)}<br><br>
 * Usage:<br>
 * - declare a <code>java.util.List</code> variable on your persistent classes.<br>
 * - fill this variable with a method in the ObjectContainer collection factory.<br><br>
 * <b>Example:</b><br><br>
 * <code>class MyClass{<br>
 * &nbsp;&nbsp;List myList;<br>
 * }<br><br>
 * MyClass myObject = new MyClass();<br> 
 * myObject.myList = objectContainer.ext().collections().newLinkedList();
 * @see com.db4o.ext.ExtObjectContainer#collections
 */
public interface Db4oList extends Db4oCollection {

}
