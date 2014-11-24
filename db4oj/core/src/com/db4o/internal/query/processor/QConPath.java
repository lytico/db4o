/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;


/** 
 * Placeholder for a constraint, only necessary to attach children
 * to the query graph.
 * 
 * Added upon a call to Query#descend(), if there is no
 * other place to hook up a new constraint.
 * 
 * @exclude
 */
public class QConPath extends QConClass {
	
	public QConPath(){
		
	}

	QConPath(Transaction a_trans, QCon a_parent, QField a_field) {
		super(a_trans, a_parent, a_field, null);
		if(a_field != null){
			_classMetadata = a_field.getFieldType();
		}
	}
	
	public boolean canLoadByIndex() {
		return false;
	}
	
	boolean evaluate(InternalCandidate candidate) {
		if (! candidate.fieldIsAvailable()) {
			visitOnNull(candidate.getRoot());
		}
		return true;
	}
	
	void evaluateSelf() {
		// do nothing
	}

	boolean isNullConstraint() {
		return ! hasChildren();
	}

	@Override
	QConClass shareParentForClass(ReflectClass a_class, BooleanByRef removeExisting) {
        if (i_parent == null) {
            return null;
        }
		QConClass newConstraint = new QConClass(i_trans, i_parent, getField(), a_class);
		morph(removeExisting,newConstraint, a_class);
		return newConstraint;
	}

	@Override
	QCon shareParent(Object a_object, BooleanByRef removeExisting) {
        if (i_parent == null) {
            return null;
        }
        Object obj = getField().coerce(a_object);
        if(obj == No4.INSTANCE){
        	QCon falseConstraint = new QConUnconditional(i_trans, false);
            morph(removeExisting, falseConstraint, reflectClassForObject(obj));
    		return falseConstraint;
        }
        QConObject newConstraint = new QConObject(i_trans, i_parent, getField(), obj);
        morph(removeExisting, newConstraint, reflectClassForObject(obj));
		return newConstraint;
	}

	private ReflectClass reflectClassForObject(Object obj) {
		return i_trans.reflector().forObject(obj);
	}

	// Our QConPath objects are just placeholders to fields,
	// so the parents are reachable.
	// If we find a "real" constraint, we throw the QPath
	// out and replace it with the other constraint. 
    private void morph(BooleanByRef removeExisting, QCon newConstraint, ReflectClass claxx) {
        boolean mayMorph = true;
        if (claxx != null) {
        	ClassMetadata yc = i_trans.container().produceClassMetadata(claxx);
        	if (yc != null) {
        		Iterator4 i = iterateChildren();
        		while (i.moveNext()) {
        			QField qf = ((QCon) i.current()).getField();
        			if (!yc.hasField(i_trans.container(), qf.name())) {
        				mayMorph = false;
        				break;
        			}
        		}
        	}
        }
        
        if (mayMorph) {
    		Iterator4 j = iterateChildren();
    		while (j.moveNext()) {
    			newConstraint.addConstraint((QCon) j.current());
    		}
        	if(hasJoins()){
        		Iterator4 k = iterateJoins();
        		while (k.moveNext()) {
        			QConJoin qcj = (QConJoin)k.current();
        			qcj.exchangeConstraint(this, newConstraint);
        			newConstraint.addJoin(qcj);
        		}
        	}
        	i_parent.exchangeConstraint(this, newConstraint);
        	removeExisting.value = true;
        	
        } else {
        	i_parent.addConstraint(newConstraint);
        }
    }

	final boolean visitSelfOnNull() {
		return false;
	}
	
	public String toString(){
        return "QConPath " + super.toString();
	}
	
    @Override
    public void setProcessedByIndex(QCandidates candidates) {
    	if(childrenCount() <=1){
    		internalSetProcessedByIndex(null);
    	}
    }
    
    @Override
	protected boolean canResolveByFieldIndex() {
    	return true;
	}

}
