/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.reflect.*;


class CollectionUpdateDepthEntry {
    
    final ReflectClassPredicate _predicate;
    
    final int _depth;
    
    CollectionUpdateDepthEntry(ReflectClassPredicate predicate,int depth) {
        _predicate = predicate;
        _depth = depth;
    }
    
}
