/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;

/**
 * @exclude
 */
public final class SearchTarget {
    
    public static final SearchTarget LOWEST = new SearchTarget("Lowest");
    
    public static final SearchTarget ANY = new SearchTarget("Any");
    
    public static final SearchTarget HIGHEST = new SearchTarget("Highest");
    
    private final String _target;

    
    public SearchTarget(String target) {
        _target = target;
    }

    public String toString() {
        return _target;
    }
    
}
