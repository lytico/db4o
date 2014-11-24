/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class QEIdentity extends QEEqual
{
	public int i_objectID;
	
	boolean identity(){
		return true;
	}
	
	boolean evaluate(QConObject a_constraint, QCandidate a_candidate, Object a_value){
		if(i_objectID == 0){
			i_objectID = a_constraint.getObjectID();
		}
		return a_candidate._key == i_objectID;
	}
}
