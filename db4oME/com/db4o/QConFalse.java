package com.db4o;

import com.db4o.foundation.*;

/** 
 * @exclude
 */
public class QConFalse extends QConPath {
	public QConFalse(){
	}
	
	QConFalse(Transaction a_trans, QCon a_parent, QField a_field) {
		super(a_trans,a_parent,a_field);
	}
	
	void createCandidates(Collection4 a_candidateCollection) {
	}
	
	boolean evaluate(QCandidate a_candidate) {
		return false;
	}
}
