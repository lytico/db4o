/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.types;

/**
 *  db4o Map implementation for database-aware maps.
 * <br><br>
 * A <code>Db4oMap</code> supplies the methods specified in java.util.Map.<br><br>
 * All access to the map is controlled by the {@link com.db4o.ObjectContainer ObjectContainer} to help the
 * programmer produce expected results with as little work as possible:<br>  
 * - newly added objects are automatically persisted.<br>
 * - map elements are automatically activated when they are needed. The activation
 * depth is configurable with {@link Db4oCollection#activationDepth(int)}.<br>
 * - removed objects can be deleted automatically, if the list is configured
 * with {@link Db4oCollection#deleteRemoved(boolean)}<br><br>
 * Usage:<br>
 * - declare a <code>java.util.Map</code> variable on your persistent classes.<br>
 * - fill this variable with a method in the ObjectContainer collection factory.<br><br>
 * <b>Example:</b><br><br>
 * <code>class MyClass{<br>
 * &nbsp;&nbsp;Map myMap;<br>
 * }<br><br>
 * MyClass myObject = new MyClass();<br> 
 * myObject.myMap = objectContainer.ext().collections().newHashMap();
 * @see com.db4o.ext.ExtObjectContainer#collections
 */
public interface Db4oMap extends Db4oCollection {

}
