/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

public class NativeQueryOptimizerNotLoaded extends DiagnosticBase {

	private int _reason;
	private final Exception _details;
	public final static int NQ_NOT_PRESENT 			= 1;
	public final static int NQ_CONSTRUCTION_FAILED 	= 2;

	
	public NativeQueryOptimizerNotLoaded(int reason, Exception details) {
		_reason = reason;
		_details = details;
	}
	public String problem() {
		return "Native Query Optimizer could not be loaded";
	}

	public Object reason() {
		switch (_reason) {
		case NQ_NOT_PRESENT:
			return AppendDetails("Native query not present.");
		case NQ_CONSTRUCTION_FAILED:
			return AppendDetails("Native query couldn't be instantiated.");
		default:
			return AppendDetails("Reason not specified.");
		}
	}

	public String solution() {
		return "If you to have the native queries optimized, please check that the native query jar is present in the class-path.";
	}

	private Object AppendDetails(String reason) {
		if (_details == null) {
			return reason;
		}
		
		return reason + "\n" + _details.toString();
	}
}
