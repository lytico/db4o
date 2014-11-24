/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public interface Procedure4<T> {
    void apply(T value);
}
