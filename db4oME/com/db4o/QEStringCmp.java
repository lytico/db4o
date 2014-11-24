/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public abstract class QEStringCmp extends QEAbstract {
	private boolean caseSensitive;

	public QEStringCmp(boolean caseSensitive) {
		this.caseSensitive = caseSensitive;
	}

	boolean evaluate(QConObject a_constraint, QCandidate a_candidate, Object a_value){
		if(a_value != null){
		    if(a_value instanceof YapReader) {
		        a_value = ((YapReader)a_value).toString(a_constraint.i_trans);
		    }
		    String candidate=a_value.toString();
		    String constraint=a_constraint.i_object.toString();
		    if(!caseSensitive) {
		    	candidate=candidate.toLowerCase();
		    	constraint=constraint.toLowerCase();
		    }
			return compareStrings(candidate,constraint);
		}
		return a_constraint.i_object.equals(null);
	}
	
	public boolean supportsIndex(){
	    return false;
	}
	
	protected abstract boolean compareStrings(String candidate,String constraint);
}
