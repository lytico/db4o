/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

import com.db4o.query.*;

/**
 * Diagnostic, if Native Query can not be run optimized.
 */
public class NativeQueryNotOptimized extends DiagnosticBase{
    
    private final Predicate _predicate;
    private final Exception _details;
    
    public NativeQueryNotOptimized(Predicate predicate, Exception details) {
        _predicate = predicate;
        _details = details;
    }

    public Object reason() {
    	if (_details == null) return _predicate;
    	return _predicate != null ? _predicate.toString() : ""  + "\n" + _details.getMessage();
    }

    public String problem() {
        return "Native Query Predicate could not be run optimized";
    }

    public String solution() {
        return "This Native Query was run by instantiating all objects of the candidate class. "
        + "Consider simplifying the expression in the Native Query method. If you feel that "
        + "the Native Query processor should understand your code better, you are invited to "
        + "post yout query code to db4o forums at http://developer.db4o.com/forums";
    }

}
