package com.db4odoc.typehandling.notstored;

/**
 * @author roman.stoffel@gamlor.info
 * @since 17.01.12
 */
// #example: Mark a field as transient
public class StoredEntity {
    // won't be stored
    private transient int someCachedValue;

    // ..
}
// #end example
