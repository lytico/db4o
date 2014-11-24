/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;


/**
 * provides methods to configure the behaviour of db4o diagnostics.
 * <br><br>Diagnostic system can be enabled on a running db4o database 
 * to notify a user about possible problems or misconfigurations.
 * Diagnostic listeners can be be added and removed with calls
 * to this interface.
 * To install the most basic listener call:<br>
 * <code>
 * EmbeddedConfiguration config = Db4oEmbedded.newConfiguration(); <br>
 * config.common().diagnostic().addListener(new DiagnosticToConsole());</code>
 * @see com.db4o.config.Configuration#diagnostic()
 * @see DiagnosticListener
 */
public interface DiagnosticConfiguration {
    
    /**
     * adds a DiagnosticListener to listen to Diagnostic messages.
     */
    public void addListener(DiagnosticListener listener);
    
    /**
     * removes all DiagnosticListeners.
     */
    public void removeAllListeners();
}
