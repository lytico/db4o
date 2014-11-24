/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

import java.util.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class JdkCollectionIterable4 implements Iterable4{
    
    private final Collection _collection;
    
    public JdkCollectionIterable4(Collection collection){
        _collection = collection;
    }

    public Iterator4 iterator() {
        return new JdkCollectionIterator4(_collection);
    }

}
