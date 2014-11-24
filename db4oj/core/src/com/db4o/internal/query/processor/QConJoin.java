/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;


/**
 * 
 * Join constraint on queries
 * 
 * @exclude
 */
public class QConJoin extends QCon {
	
	// FIELDS MUST BE PUBLIC TO BE REFLECTED ON UNDER JDK <= 1.1

	@decaf.Public
    private boolean i_and;
	
	@decaf.Public
    private QCon i_constraint1;
	
	@decaf.Public
    private QCon i_constraint2;
	
	
	public QConJoin(){
		// C/S
	}

	QConJoin(Transaction a_trans, QCon a_c1, QCon a_c2, boolean a_and) {
		super(a_trans);
		i_constraint1 = a_c1;
		i_constraint2 = a_c2;
		i_and = a_and;
	}

	public QCon constraint2() {
	    return i_constraint2;
    }

	public QCon constraint1() {
	    return i_constraint1;
    }

	void doNotInclude(InternalCandidate root) {
		constraint1().doNotInclude(root);
		constraint2().doNotInclude(root);
	}

	void exchangeConstraint(QCon a_exchange, QCon a_with) {
		super.exchangeConstraint(a_exchange, a_with);
		if (a_exchange == constraint1()) {
			i_constraint1 = a_with;
		}
		if (a_exchange == constraint2()) {
			i_constraint2 = a_with;
		}
	}

	void evaluatePending(InternalCandidate root, QPending pending, int secondResult) {
		boolean res =
			i_evaluator.not(
				i_and
					? ((pending._result + secondResult) > 0)
					: (pending._result + secondResult) > QPending.FALSE);
					
		if (hasJoins()) {
			Iterator4 i = iterateJoins();
			while (i.moveNext()) {
				QConJoin qcj = (QConJoin) i.current();
				if (Debug4.queries) {
					System.out.println(
						"QConJoin creates pending this:"
							+ id()
							+ " Join:"
							+ qcj.id()
							+ " res:"
							+ res);
				}
				root.evaluate(new QPending(qcj, this, res));
			}
		} else {
			if (!res) {
				if (Debug4.queries) {
					System.out.println(
						"QConJoin evaluatePending FALSE for " + root.id() + " "
							+ id()
							+ " doNotInclude: "
							+ constraint1().id()
							+ ", "
							+ constraint2().id());
				}
				constraint1().doNotInclude(root);
				constraint2().doNotInclude(root);
			}else{
				if (Debug4.queries) {
					System.out.println(
						"QConJoin evaluatePending TRUE for " + root.id() + " "
							+ id()
							+ " keeping constraints: "
							+ constraint1().id()
							+ ", "
							+ constraint2().id());
				}
			}

		}
	}

	public QCon getOtherConstraint(QCon a_constraint) {
		if (a_constraint == constraint1()) {
			return constraint2();
		} else if (a_constraint == constraint2()) {
			return constraint1();
		}
		throw new IllegalArgumentException();
	}
	
	String logObject(){
		if (Debug4.queries) {
			String msg = i_and ? "&" : "|";
			return " " + constraint1().id() + msg + constraint2().id();
		}
		return "";
	}
	
	public String toString(){
		String str = "QConJoin " + (i_and ? "AND ": "OR");
		if(constraint1() != null){
			str += "\n   " + constraint1();  
		}
		if(constraint2() != null){
			str += "\n   " + constraint2();  
		}
		return str;
	}

	public boolean isOr() {
		return !i_and;
	}
	
	public void setProcessedByIndex(QCandidates candidates) {
		if(processedByIndex()){
			return;
		}
		super.setProcessedByIndex(candidates);
		constraint1().setProcessedByIndex(candidates);
		constraint2().setProcessedByIndex(candidates);
	}

	@Override
	protected boolean canResolveByFieldIndex() {
		return false;
	}

}
