/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

/**
 * Diagnostic, if query was required to load candidate set from class index.
 */
public class LoadedFromClassIndex extends DiagnosticBase{
    
    private final String _className;
    
    
    public LoadedFromClassIndex(String className) {
        _className = className;
    }

    public Object reason() {
        return _className;
    }

    public String problem() {
        return "Query candidate set could not be loaded from a field index";
    }

    public String solution() {
        return "Consider indexing fields that you query for: "
            + "configuration.common().objectClass("+_className+").objectField([fieldName]).indexed(true)" ;
    }

}
