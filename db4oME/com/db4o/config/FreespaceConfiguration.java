/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.config;

/**
 * interface to configure the freespace system to be used.
 * All methods should be called before opening database files.
 * If db4o is instructed to exchange the system 
 * ( {@link #useIndexSystem()} , {@link #useRamSystem()} )
 * this will happen on opening the database file.<br><br>
 * By default the index-based system will be used.  
 */
public interface FreespaceConfiguration {
    
    /**
     * tuning feature: configures the minimum size of free space slots in the database file 
     * that are to be reused.
     * <br><br>When objects are updated or deleted, the space previously occupied in the
     * database file is marked as "free", so it can be reused. db4o maintains two lists
     * in RAM, sorted by address and by size. Adjacent entries are merged. After a large
     * number of updates or deletes have been executed, the lists can become large, causing
     * RAM consumption and performance loss for maintenance. With this method you can 
     * specify an upper bound for the byte slot size to discard. 
     * <br><br>Pass <code>Integer.MAX_VALUE</code> to this method to discard all free slots for
     * the best possible startup time.<br><br>
     * The downside of setting this value: Database files will necessarily grow faster. 
     * <br><br>Default value:<br>
     * <code>0</code> all space is reused
     * @param byteCount Slots with this size or smaller will be lost.
     */
    public void discardSmallerThan(int byteCount);
    
    /**
     * configures db4o to use an index-based freespace system.
     * <br><br><b>Advantages</b><br>
     * - ACID, no freespace is lost on abnormal system termination<br>
     * - low memory consumption<br>
     * <br><b>Disadvantages</b><br>
     * - slower than the RAM-based system, since freespace information
     * is written during every commit<br>
     */
    public void useIndexSystem(); 

    /**
     * configures db4o to use a RAM-based freespace system.
     * <br><br><b>Advantages</b><br>
     * - best performance<br>
     * <br><b>Disadvantages</b><br>
     * - upon abnormal system termination all freespace is lost<br>
     * - memory consumption<br>
     */
    public void useRamSystem();
    
}
