/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

/**
 * Diagnostic, if update depth greater than 1.
 */
public class UpdateDepthGreaterOne extends DiagnosticBase{
    
    private final int _depth;
    
    public UpdateDepthGreaterOne(int depth) {
        _depth = depth;
    }

    public Object reason() {
        return "configuration.common().configure().updateDepth(" + _depth + ")";
    }

    public String problem() {
        return "A global update depth greater than 1 is not recommended";
    }

    public String solution() {
        return "Increasing the global update depth to a value greater than 1 is only recommended for"
            + " testing, not for production use. If individual deep updates are needed, consider using"
            + " ExtObjectContainer#set(object, depth) and make sure to profile the performance of each call.";
    }

}
