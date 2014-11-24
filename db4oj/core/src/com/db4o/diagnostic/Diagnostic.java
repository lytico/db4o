/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

/**
 * Marker interface for Diagnostic messages<br><br>
 * Diagnostic system can be enabled on a running db4o database 
 * to notify a user about possible problems or misconfigurations. Diagnostic
 * messages must implement this interface and are usually derived from
 * {@link DiagnosticBase} class. A separate Diagnostic implementation
 * should be used for each problem.
 * @see DiagnosticBase
 * @see DiagnosticConfiguration
 */
public interface Diagnostic {

}
