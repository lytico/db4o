/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;

/**
 * @exclude
 */
public class QEMulti extends QE{
	
	public Collection4 i_evaluators = new Collection4();
	
	QE add(QE evaluator){
		i_evaluators.ensure(evaluator);
		return this;
	}
	
	boolean identity(){
		boolean ret = false;
		Iterator4 i = i_evaluators.iterator();
		while(i.hasNext()){
			if(((QE)i.next()).identity()){
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
	
	boolean evaluate(QConObject a_constraint, QCandidate a_candidate, Object a_value){
		Iterator4 i = i_evaluators.iterator();
		while(i.hasNext()){
			if(((QE)i.next()).evaluate(a_constraint, a_candidate, a_value)){
				return true;
			}
		}
		return false;
	}
	
	public void indexBitMap(boolean[] bits){
	    Iterator4 i = i_evaluators.iterator();
	    while(i.hasNext()){
	        ((QE)i.next()).indexBitMap(bits);
	    }
	}
	
	public boolean supportsIndex(){
	    Iterator4 i = i_evaluators.iterator();
	    while(i.hasNext()){
	        if(! ((QE)i.next()).supportsIndex()){
	            return false;
	        }
	    }
	    return true;
	}
	
	
	
}

