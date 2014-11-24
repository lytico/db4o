/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class QEEndsWith extends QEStringCmp
{
	public QEEndsWith(boolean caseSensitive) {
		super(caseSensitive);
	}

	protected boolean compareStrings(String candidate, String constraint) {
		return J2MEUtil.lastIndexOf(candidate,constraint)==candidate.length()-constraint.length();
	}
}
