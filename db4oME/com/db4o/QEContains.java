/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class QEContains extends QEStringCmp
{
	public QEContains(boolean caseSensitive) {
		super(caseSensitive);
	}

	protected boolean compareStrings(String candidate, String constraint) {
		return candidate.indexOf(constraint) > -1;
	}
}
