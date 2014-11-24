/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

/**
 * Diagnostic to recommend Defragment when needed.
 */
public class DefragmentRecommendation extends DiagnosticBase{
	
	private final DefragmentRecommendationReason _reason;
	
	public DefragmentRecommendation(DefragmentRecommendationReason reason){
		_reason = reason;
	}
	
	public static class DefragmentRecommendationReason{
		
		final String _message;

		public DefragmentRecommendationReason(String reason) {
			_message = reason;
		}

		public static final DefragmentRecommendationReason DELETE_EMBEDED = 
			new DefragmentRecommendationReason("Delete Embedded not supported on old file format."); 
		
	}
	
	public String problem() {
		return "Database file format is old or database is highly fragmented.";
	}
	
	public Object reason() {
		return _reason._message;
	}
	
	public String solution() {
		return "Defragment the database.";
	}
	
}
