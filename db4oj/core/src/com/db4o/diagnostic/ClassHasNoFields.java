/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;


/**
 * Diagnostic, if class has no fields.
 */
public class ClassHasNoFields extends DiagnosticBase{
    
    private final String _className;
    
    public ClassHasNoFields(String className){
        _className = className;
    }
    
    public Object reason() {
        return _className;
    }
    
    public String problem() {
        return "Class does not contain any persistent fields";
    }

    public String solution() {
        return "Every class in the hierarchy requires overhead for the maintenance of a class index." 
        + " Consider removing this class from the hierarchy, if it is not needed.";
    }

}
