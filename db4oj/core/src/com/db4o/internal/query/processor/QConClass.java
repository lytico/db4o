/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.classindex.*;
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
	
	@decaf.Public
    private String _className;
	
	@decaf.Public
    private boolean i_equal;
	
	public QConClass(){
		// C/S
	}
	
	QConClass(Transaction trans, QCon parent, QField field, ReflectClass claxx){
		super(trans, parent, field, null);
		if(claxx != null){
			ObjectContainerBase container = trans.container();
			_classMetadata = container.classMetadataForReflectClass(claxx);
			if(_classMetadata == null){
				// could be an aliased class, try to resolve.
				String className = claxx.getName();
				String aliasRunTimeName = container.config().resolveAliasStoredName(className);
				if(! className.equals(aliasRunTimeName)){
					_classMetadata = container.classMetadataForName(aliasRunTimeName);
				}
			}
			if(claxx.equals(container._handlers.ICLASS_OBJECT)){
				_classMetadata = (ClassMetadata)_classMetadata.typeHandler();
			}
		}
		_claxx = claxx;
	}
	
	QConClass(Transaction trans, ReflectClass claxx){
	    this(trans ,null, null, claxx);
	}
	
	public String getClassName() {
		return _claxx == null ? null : _claxx.getName();
	}
    
    public boolean canBeIndexLeaf(){
        return false;
    }
	
	boolean evaluate(InternalCandidate candidate){
		boolean result = true;
		QCandidates qCandidates = candidate.candidates();
		if(qCandidates.isTopLevel() && qCandidates.wasLoadedFromClassFieldIndex()) { 
			if(_classMetadata.getAncestor() != null){
				BTreeClassIndexStrategy index = (BTreeClassIndexStrategy) _classMetadata.index();
				if(index == null){
					return i_evaluator.not(true);
				}
				BTree btree = index.btree();
				Object searchResult = btree.search(candidate.transaction(), candidate.id());
				result = searchResult != null;
			}
		} else {
			ReflectClass claxx = candidate.classMetadata().classReflector();
			if(claxx == null){
				result = false;
			}else{
				result = i_equal ? _claxx.equals(claxx) : _claxx.isAssignableFrom(claxx);
			}
		}
		return i_evaluator.not(result);
	}
	
	void evaluateSelf() {
		
		if(i_candidates.wasLoadedFromClassIndex()){
			if(i_evaluator.isDefault()){
				if(! hasJoins()){
					if(_classMetadata != null  && i_candidates._classMetadata != null){
						if(_classMetadata.getHigherHierarchy(i_candidates._classMetadata) == _classMetadata){
							return;
						}
					}
				}
			}
		}
		
		if(i_candidates.wasLoadedFromClassFieldIndex()){
			if(i_candidates.isTopLevel()) {
				if(i_evaluator.isDefault()){
					if(! hasJoins()){
						if(canResolveByFieldIndex()){
							return;
						}
					}
				}
			}
		}

		i_candidates.filter(this);
	}
	
    @Override
	protected boolean canResolveByFieldIndex() {
    	return _classMetadata != null && _classMetadata.getAncestor() == null;
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
        if (Debug4.queries) {
            if(_claxx != null){
                return _claxx.toString();
            }
        } 
        return "";
    }
    
    void marshall() {
        super.marshall();
        if(_claxx!=null) {
        	_className = container().config().resolveAliasRuntimeName(_claxx.getName());
        }
    }
	
	public String toString(){
		String str = "QConClass ";
		if(_claxx != null){
			str += _claxx.getName() + " ";
		}
		return str + super.toString();
	}
	
    void unmarshall(Transaction a_trans) {
        if (i_trans == null) {
            super.unmarshall(a_trans);
            if(_className!=null) {
            	_className = container().config().resolveAliasStoredName(_className);
            	_claxx = a_trans.reflector().forName(_className);
            }
        }
    }
    
    void setEvaluationMode() {
        Iterator4 children = iterateChildren();
        while (children.moveNext()) {
            Object child = children.current();
            if (child instanceof QConObject) {
                ((QConObject) child).setEvaluationMode();
            }
        }
    }
    
    @Override
    public void setProcessedByIndex(QCandidates candidates) {
    	// do nothing, QConClass needs to stay in the evaluation graph.
    }
    
}

