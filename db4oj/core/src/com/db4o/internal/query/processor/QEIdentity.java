/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;


/**
 * @exclude
 */
public class QEIdentity extends QEEqual
{
	@decaf.Public
    private int i_objectID;
	
	public boolean identity(){
		return true;
	}
	
	boolean evaluate(QConObject a_constraint, InternalCandidate a_candidate, Object a_value){
		if(i_objectID == 0){
			i_objectID = a_constraint.getObjectID();
		}
		return a_candidate.id() == i_objectID;
	}
}
