/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;


/**
 * @exclude
 */
public class QEEndsWith extends QEStringCmp {

	/** for C/S messaging only */
	public QEEndsWith() {
	}
	
	public QEEndsWith(boolean caseSensitive_) {
		super(caseSensitive_);
	}

	protected boolean compareStrings(String candidate, String constraint) {
		int lastIndex = candidate.lastIndexOf(constraint);
		if (lastIndex == -1) {
			return false;
		}
		return lastIndex == candidate.length() - constraint.length();
	}
}
