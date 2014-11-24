/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

import com.db4o.*;


/**
 * base class for Diagnostic messages
 */
public abstract class DiagnosticBase implements Diagnostic{
    
    /**
     * returns the reason for the message 
     */
    public abstract Object reason();  
    
    /**
     * returns the potential problem that triggered the message
     */
    public abstract String problem();
    
    /**
     * suggests a possible solution for the possible problem
     */
    public abstract String solution();
    
    public String toString() {
        return ":: db4o " + Db4oVersion.NAME + " Diagnostics ::\n  " + reason() + " :: " + problem() + "\n    " + solution(); 
    }

}
