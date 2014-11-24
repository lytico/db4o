/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.internal.*;
import com.db4o.internal.handlers.*;


/**
 * @exclude
 */
public abstract class QEStringCmp extends QEAbstract {
    
	@decaf.Public
    private boolean caseSensitive;

	/** for C/S messaging only */
	public QEStringCmp() {
	}
	
	public QEStringCmp(boolean caseSensitive_) {
		caseSensitive = caseSensitive_;
	}

	boolean evaluate(QConObject constraint, InternalCandidate candidate, Object obj){
		if(obj != null){
		    if(obj instanceof ByteArrayBuffer) {
		    	obj = StringHandler.readString(candidate.transaction().context(), (ByteArrayBuffer)obj);
		    }
		    String candidateStringValue = obj.toString();
		    String stringConstraint = constraint.getObject().toString();
		    if(!caseSensitive) {
		    	candidateStringValue=candidateStringValue.toLowerCase();
		    	stringConstraint=stringConstraint.toLowerCase();
		    }
			return compareStrings(candidateStringValue,stringConstraint);
		}
		return constraint.getObject()==null;
	}
	
	public boolean supportsIndex(){
	    return false;
	}
	
	protected abstract boolean compareStrings(String candidate,String constraint);
}
