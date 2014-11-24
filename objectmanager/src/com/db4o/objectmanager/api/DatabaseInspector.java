package com.db4o.objectmanager.api;

import com.db4o.ReplicationRecord;
import com.db4o.MetaClass;

import java.util.List;

/**
 * User: treeder
 * Date: Aug 7, 2006
 * Time: 10:25:13 AM
 */
public interface DatabaseInspector {
    /**
     *
     * @return the number of unique classes stored in the database
     */
    int getNumberOfClasses();

    /**
     *
     * @return a list of all the different class types stored
     * todo: Is MetaClass the correct thing to return here?  or maybe something new like a ClassMetaData
     */
    List getClassesStored();

    /**
     *
     * @param aClass the type of object
     * @return the number of objects stored of type aClass
     */
    int getNumberOfObjectsForClass(Class aClass);

    /**
     *
     * @param aClass the fully qualified class name
     * @return the number of objects stored of type aClass
     * @see #getNumberOfObjectsForClass(Class)
     */
    int getNumberOfObjectsForClass(String aClass);

    /**
     * 
     * @return the <b>total</b> disk space in bytes
     */
    long getSpaceUsed();

    /**
     *
     * @return the disk space used by by indexes in bytes
     */
    long getSpaceUsedByIndexes();

    /**
     *
     * @return the disk space used by class meta data in bytes
     */
    long getSpaceUsedByClassMetaData();

    /**
     *
     * @return the disk space used by USER stored objects in bytes
     */
    long getSpaceUsedByStoredObjects();

    /**
     * Space that has been acquired, but not used.
     * aka: free space
     *
     * @return space not used in bytes
     */
    long getSpaceNotUsed();

    /**
     * Unallocated space is space that is not used and cannot be reused.
     * (lost space that can't be attributed)
     * 
     * @return unallocated space in bytes
     */
    long getSpaceUnallocated();

    /**
     *
     * @return the total number of indexes in the database
     */
    int getNumberOfIndexes();

    /**
     *
     * @return a collection of IndexStats representing <b>all</b> indexes in the database
     */
    List getIndexStats();

    /**
     *
     * @return a collection of <b>all</b> replication records for this database
     */
    List getReplicationRecords();


    
}
