/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;


/**
 * @exclude
 */
public class QEContains extends QEStringCmp
{
	/** for C/S messaging only */
	public QEContains() {
	}
	
	public QEContains(boolean caseSensitive_) {
		super(caseSensitive_);
	}

	protected boolean compareStrings(String candidate, String constraint) {
		return candidate.indexOf(constraint) > -1;
	}
}
