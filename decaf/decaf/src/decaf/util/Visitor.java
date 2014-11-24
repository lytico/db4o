/* Copyright (C) 2011  Versant Inc.  http://www.db4o.com */

package decaf.util;

public interface Visitor<T> {
    void visit(T value);
}
