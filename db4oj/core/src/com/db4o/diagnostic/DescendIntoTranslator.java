/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.diagnostic;

/**
 * Query tries to descend into a field of a class that is configured to be translated
 * (and thus cannot be descended into).
 */
public class DescendIntoTranslator extends DiagnosticBase {
	private String className;
	private String fieldName;
	
	public DescendIntoTranslator(String className_, String fieldName_) {
		className = className_;
		fieldName = fieldName_;
	}

	public String problem() {
		return "Query descends into field(s) of translated class.";
	}

	public Object reason() {
		return className+"."+fieldName;
	}

	public String solution() {
		return "Consider dropping the translator configuration or resort to evaluations/unoptimized NQs.";
	}
}
