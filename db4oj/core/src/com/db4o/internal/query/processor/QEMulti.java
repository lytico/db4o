/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.foundation.*;


/**
 * @exclude
 */
public class QEMulti extends QE{
	
	@decaf.Public
    private Collection4 i_evaluators = new Collection4();
	
	// used by .net LINQ tests
	public Iterable4 evaluators() {
		return i_evaluators;
	}
	
	QE add(QE evaluator){
		i_evaluators.ensure(evaluator);
		return this;
	}
	
	public boolean identity(){
		boolean ret = false;
		Iterator4 i = i_evaluators.iterator();
		while(i.moveNext()){
			if(((QE)i.current()).identity()){
				ret = true;
			}else{
				return false;
			}
		}
		return ret;
	}
    
    boolean isDefault(){
        return false;
    }
	
	boolean evaluate(QConObject a_constraint, InternalCandidate a_candidate, Object a_value){
		Iterator4 i = i_evaluators.iterator();
		while(i.moveNext()){
			if(((QE)i.current()).evaluate(a_constraint, a_candidate, a_value)){
				return true;
			}
		}
		return false;
	}
	
	public void indexBitMap(boolean[] bits){
	    Iterator4 i = i_evaluators.iterator();
	    while(i.moveNext()){
	        ((QE)i.current()).indexBitMap(bits);
	    }
	}
	
	public boolean supportsIndex(){
	    Iterator4 i = i_evaluators.iterator();
	    while(i.moveNext()){
	        if(! ((QE)i.current()).supportsIndex()){
	            return false;
	        }
	    }
	    return true;
	}
	
	
	
}

