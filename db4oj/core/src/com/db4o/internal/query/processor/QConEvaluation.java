/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.internal.*;


/**
 * @exclude
 */
public class QConEvaluation extends QCon {

	private transient Object i_evaluation;

	@decaf.Public
    private byte[] i_marshalledEvaluation;

	@decaf.Public
    private int i_marshalledID;

	public QConEvaluation() {
		// C/S only
	}

	public QConEvaluation(Transaction a_trans, Object a_evaluation) {
		super(a_trans);
		i_evaluation = a_evaluation;
	}

	void evaluateEvaluationsExec(QCandidates a_candidates, boolean rereadObject) {
//		if (rereadObject) {
//			a_candidates.traverse(new Visitor4() {
//				public void visit(Object a_object) {
//					((QCandidate) a_object).useField(null);
//				}
//			});
//		}
		a_candidates.filter(this);
	}

    void marshall() {
        super.marshall();
		if(!Platform4.useNativeSerialization()){
			marshallUsingDb4oFormat();
		}else{
    		try{
    			i_marshalledEvaluation = Platform4.serialize(i_evaluation);
    		}catch (Exception e){
    			marshallUsingDb4oFormat();
    		}
		}
	}
    
    private void marshallUsingDb4oFormat(){
    	SerializedGraph serialized = Serializer.marshall(container(), i_evaluation);
    	i_marshalledEvaluation = serialized._bytes;
    	i_marshalledID = serialized._id;
    }

    void unmarshall(Transaction a_trans) {
    	if (i_trans == null) {
    		super.unmarshall(a_trans);
    		
            if(i_marshalledID > 0 || !Platform4.useNativeSerialization()){
            	i_evaluation = Serializer.unmarshall(container(), i_marshalledEvaluation, i_marshalledID);
            }else{
                i_evaluation = Platform4.deserialize(i_marshalledEvaluation);
            }
        }
    }

	public void visit(Object obj) {
		InternalCandidate candidate = (InternalCandidate) obj;
		
		// force activation outside the try block
		// so any activation errors bubble up
		forceActivation(candidate); 
		
		try {
			Platform4.evaluationEvaluate(i_evaluation, candidate);
		} catch (Exception e) {
			candidate.include(false);
			// TODO: implement Exception callback for the user coder
			// at least for test cases
		}
		if (!candidate.include()) {
			doNotInclude(candidate.getRoot());
		}
	}

	private void forceActivation(InternalCandidate candidate) {
		candidate.getObject();
	}

	boolean supportsIndex() {
		return false;
	}

	@Override
	protected boolean canResolveByFieldIndex() {
		return false;
	}
}
