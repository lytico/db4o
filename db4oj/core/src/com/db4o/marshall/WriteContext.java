/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.marshall;

import com.db4o.typehandlers.*;


/**
 * this interface is passed to internal class {@link TypeHandler4} during marshaling
 * and provides methods to marshal objects. 
 */
public interface WriteContext extends Context, WriteBuffer {

    /**
     * makes sure the object is stored and writes the ID of
     * the object to the context.
     * Use this method for first class objects only (objects that
     * have an identity in the database). If the object can potentially
     * be a primitive type, do not use this method but use 
     * a matching {@link WriteBuffer} method instead.
     * @param obj the object to write.
     */
    void writeObject(Object obj);

    /**
     * writes sub-objects, in cases where the {@link TypeHandler4} is known.
     * 
     * @param handler typehandler to be used to write the object.
     * @param obj the object to write
     */
    void writeObject(TypeHandler4 handler, Object obj);

    /**
     * reserves a buffer with a specific length at the current
     * position, to be written in a later step.
     * @param length the length to be reserved. 
     * @return the ReservedBuffer
     */
    ReservedBuffer reserve(int length);
    
}
