/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.inside.ix.*;

/**
 * @exclude
 */
public class QEGreater extends QEAbstract
{
	boolean evaluate(QConObject a_constraint, QCandidate a_candidate, Object a_value){
		if(a_value == null){
			return false;
		}
		return a_constraint.getComparator(a_candidate).isGreater(a_value);
	}
	
	public void indexBitMap(boolean[] bits){
	    bits[IxTraverser.GREATER] = true;
	}
}
