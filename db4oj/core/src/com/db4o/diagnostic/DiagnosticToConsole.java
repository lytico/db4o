/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;


/**
 * prints Diagnostic messsages to the Console.
 * Install this {@link DiagnosticListener} with: <br>
 * <code>
 * EmbeddedConfiguration config = Db4oEmbedded.newConfiguration(); <br>
 * config.common().diagnostic().addListener(new DiagnosticToConsole());</code><br>
 * @see DiagnosticConfiguration
 */
public class DiagnosticToConsole implements DiagnosticListener{

    /**
     * redirects Diagnostic messages to the Console.
     */
    public void onDiagnostic(Diagnostic d) {
        System.out.println(d.toString());
    }
    
}
