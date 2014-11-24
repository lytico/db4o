/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.classindex.*;
import com.db4o.internal.fieldindex.*;

/**
 * @exclude
 */
public class QueryResultCandidates {
	
	private QCandidateBase _candidates;

	private QCandidates _qCandidates;
	
	IntVisitable _candidateIds;
	
	
	public QueryResultCandidates(QCandidates qCandidates){
		_qCandidates = qCandidates;
	}

	public void add(InternalCandidate candidate) {
		toQCandidates();
		_candidates = Tree.add(_candidates, (QCandidateBase)candidate);
	}

	private void toQCandidates() {
		if(_candidateIds == null){
			return;
		}
		_candidateIds.traverse(new IntVisitor() {
			public void visit(int id) {
				_candidates = Tree.add(_candidates, new QCandidate(_qCandidates, null, id));
			}
		});
		_candidateIds = null;
	}

	public void fieldIndexProcessorResult(FieldIndexProcessorResult result) {
		_candidateIds = result;
	}

	public void singleCandidate(QCandidateBase candidate) {
		_candidates = candidate;
		_candidateIds = null;
	}
	
    boolean filter(Visitor4 visitor) {
    	toQCandidates();
        if (_candidates != null) {
        	_candidates.traverse(visitor);
        	_candidates = (QCandidateBase) _candidates.filter(new Predicate4() {
                public boolean match(Object a_candidate) {
                    return ((QCandidateBase) a_candidate)._include;
                }
            });
        }
        return _candidates != null;
    }

    boolean filter(final QField field, final FieldFilterable filterable) {
    	toQCandidates();
        if (_candidates != null) {
        	_candidates.traverse(new Visitor4() {
				@Override
				public void visit(Object candidate) {
					filterable.filter(field, (ParentCandidate)candidate);
				}
        	});
        	_candidates = (QCandidateBase) _candidates.filter(new Predicate4() {
                public boolean match(Object a_candidate) {
                    return ((QCandidateBase) a_candidate)._include;
                }
            });
        }
        return _candidates != null;
    }

	public void loadFromClassIndex(ClassIndexStrategy index) {
		_candidateIds = index.idVisitable(_qCandidates.transaction());
	}
	
    void traverse(Visitor4 visitor) {
    	toQCandidates();
        if(_candidates != null){
            _candidates.traverse(visitor);
        }
    }

	public void traverseIds(final IntVisitor visitor) {
		if(_candidateIds != null){
			_candidateIds.traverse(visitor);
			return;
		}
		traverse(new Visitor4() {
			public void visit(Object obj) {
				QCandidateBase candidate = (QCandidateBase) obj;
				if(candidate.include()){
					visitor.visit(candidate._key);
				}
			}
		});
		
	}


}
