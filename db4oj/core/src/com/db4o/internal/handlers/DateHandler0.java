/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers;

import java.util.*;

import com.db4o.marshall.*;


/**
 * @exclude
 */
public class DateHandler0 extends DateHandler{

    public Object read(ReadContext context) {
        final long value = context.readLong();
        if (value == Long.MAX_VALUE) {
            return primitiveNull();
        }
        return new Date(value);
    }

}
