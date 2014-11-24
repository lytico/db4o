/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.instrumentation.core;

/**
 * Used to report success/status after applying an instrumentation step.
 */
public class InstrumentationStatus {

	public final static InstrumentationStatus FAILED = new InstrumentationStatus(false, false, "FAILED");
	public final static InstrumentationStatus INSTRUMENTED = new InstrumentationStatus(true, true, "INSTRUMENTED");
	public final static InstrumentationStatus NOT_INSTRUMENTED = new InstrumentationStatus(true, false, "NOT INSTRUMENTED");
	
	private final boolean _canContinue;
	private final boolean _isInstrumented;
	private final String _name;
	
	private InstrumentationStatus(final boolean canContinue, final boolean isInstrumented, String name) {
		_canContinue = canContinue;
		_isInstrumented = isInstrumented;
		_name = name;
	}
	
	public boolean canContinue() {
		return _canContinue;
	}
	
	public boolean isInstrumented() {
		return _isInstrumented;
	}
	
	public InstrumentationStatus aggregate(InstrumentationStatus status, boolean ignoreFailure) {
		if(!(ignoreFailure || (_canContinue && status._canContinue))) {
			return FAILED;
		}
		if(_isInstrumented || status._isInstrumented) {
			return INSTRUMENTED;
		}
		return NOT_INSTRUMENTED;
	}
	
	public String toString() {
		return _name;
	}
}
