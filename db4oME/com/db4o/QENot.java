/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class QENot extends QE{
	
	public QE i_evaluator;
    
    public QENot(){
        // CS
    }
	
	QENot(QE a_evaluator){
		i_evaluator = a_evaluator;
	}
	
	QE add(QE evaluator){
		if(! (evaluator instanceof QENot)){
			i_evaluator = i_evaluator.add(evaluator);
		}
		return this;
	}
	
	boolean identity(){
		return i_evaluator.identity();
	}
    
    boolean isDefault(){
        return false;
    }
	
	boolean evaluate(QConObject a_constraint,  QCandidate a_candidate, Object a_value){
		return ! i_evaluator.evaluate(a_constraint, a_candidate, a_value);
	}
	
	boolean not(boolean res){
		return ! res;
	}
	
	public void indexBitMap(boolean[] bits){
	    i_evaluator.indexBitMap(bits);
	    for (int i = 0; i < 4; i++) {
            bits[i] = ! bits[i];
        }
	}
	
	public boolean supportsIndex(){
	    return i_evaluator.supportsIndex();
	}
}

