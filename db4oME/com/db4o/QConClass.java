/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.query.*;
import com.db4o.reflect.*;

/**
 *
 * Class constraint on queries
 * 
 * @exclude
 */
public class QConClass extends QConObject{
	
	private transient ReflectClass _claxx;
	public String _className;
	public boolean i_equal;
	
	public QConClass(){
		// C/S
	}
	
	QConClass(Transaction a_trans, QCon a_parent, QField a_field, ReflectClass claxx){
		super(a_trans, a_parent, a_field, null);
		if(claxx != null){
			i_yapClass = a_trans.i_stream.getYapClass(claxx, true);
			if(claxx.equals(a_trans.i_stream.i_handlers.ICLASS_OBJECT)){
				i_yapClass = (YapClass)((YapClassPrimitive)i_yapClass).i_handler;
			}
		}
		_claxx = claxx;
	}
    
    public boolean canBeIndexLeaf(){
        return false;
    }
	
	boolean evaluate(QCandidate a_candidate){
		boolean res = true;
		ReflectClass claxx = a_candidate.classReflector();
		if(claxx == null){
			res = false;
		}else{
			res = i_equal ? _claxx.equals(claxx) : _claxx.isAssignableFrom(claxx);
		}
		return i_evaluator.not(res);
	}
	
	void evaluateSelf() {
		
		// optimization for simple class queries: 
		// No instantiation of objects, if not necessary.
		// Does not handle the special comparison of the
		// Compare interface.
		// 
		if(i_evaluator.isDefault()){
			if(i_orderID == 0 && ! hasJoins()){
				if(i_yapClass != null  && i_candidates.i_yapClass != null){
					if(i_yapClass.getHigherHierarchy(i_candidates.i_yapClass) == i_yapClass){
						return;
					}
				}
			}
		}
		i_candidates.filter(this);
	}
	
	public Constraint equal (){
		synchronized(streamLock()){
			i_equal = true;
			return this;
		}
	}
	
	boolean isNullConstraint() {
		return false;
	}
    
    String logObject() {
        if (Deploy.debugQueries) {
            if(_claxx != null){
                return _claxx.toString();
            }
        } 
        return "";
    }
    
    void marshall() {
        super.marshall();
        if(_claxx!=null) {
        	_className = _claxx.getName();
        }
    }
	
	public String toString(){
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
		String str = "QConClass ";
		if(_claxx != null){
			str += _claxx.toString() + " ";
		}
		return str + super.toString();
	}
	
    void unmarshall(Transaction a_trans) {
        if (i_trans == null) {
            super.unmarshall(a_trans);
            if(_className!=null) {
            	_claxx = a_trans.reflector().forName(_className);
            }
        }
    }
    
}

