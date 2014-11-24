/**
 * 
 */
package com.db4o.db4ounit.common.api;

import java.util.*;

import com.db4o.diagnostic.*;

import db4ounit.*;

final class DiagnosticCollector implements DiagnosticListener {
	
	ArrayList<Diagnostic> _diagnostics = new ArrayList<Diagnostic>();
	
	public void onDiagnostic(Diagnostic d) {
		_diagnostics.add(d);
	}

	public void verify(Object... expected) {
		ArrayAssert.areEqual(expected, _diagnostics.toArray());
	}
}