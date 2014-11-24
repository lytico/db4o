package com.db4o.objectmanager.api;

/**
 * User: treeder
 * Date: Aug 7, 2006
 * Time: 10:35:02 AM
 */
public interface IndexStats {
    /**
     *
     * @return the class that the index is on
     */
    Class getIndexedClass();

    /**
     * The fieldName is the string representation of the particular field the index is on.
     *
     * @return the name of the field that the index is on.  Returns null if the index is a class index (id index).
     */
    String getFieldName();

    /**
     *
     * @return the number of index entries in this index
     */
    long getNumberOfEntries();

    /**
     *
     * @return type of index
     */
    IndexType getIndexType();
    
}
