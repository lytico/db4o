/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;

/**
 * 
 * Join constraint on queries
 * 
 * @exclude
 */
public class QConJoin extends QCon {

	public boolean i_and;
	public QCon i_constraint1;
	public QCon i_constraint2;
	
	
	public QConJoin(){
		// C/S
	}

	QConJoin(Transaction a_trans, QCon a_c1, QCon a_c2, boolean a_and) {
		super(a_trans);
		i_constraint1 = a_c1;
		i_constraint2 = a_c2;
		i_and = a_and;
	}

	void doNotInclude(QCandidate a_root) {
		i_constraint1.doNotInclude(a_root);
		i_constraint2.doNotInclude(a_root);
	}

	void exchangeConstraint(QCon a_exchange, QCon a_with) {
		super.exchangeConstraint(a_exchange, a_with);
		if (a_exchange == i_constraint1) {
			i_constraint1 = a_with;
		}
		if (a_exchange == i_constraint2) {
			i_constraint2 = a_with;
		}
	}

	void evaluatePending(
		QCandidate a_root,
		QPending a_pending,
		QPending a_secondPending,
		int a_secondResult) {

		boolean res =
			i_evaluator.not(
				i_and
					? ((a_pending._result + a_secondResult) > 0)
					: (a_pending._result + a_secondResult) > -4);
					
		if (hasJoins()) {
			Iterator4 i = iterateJoins();
			while (i.hasNext()) {
				QConJoin qcj = (QConJoin) i.next();
				if (Deploy.debugQueries) {
					System.out.println(
						"QConJoin creates pending this:"
							+ i_id
							+ " Join:"
							+ qcj.i_id
							+ " res:"
							+ res);
				}
				a_root.evaluate(new QPending(qcj, this, res));
			}
		} else {
			if (!res) {
				if (Deploy.debugQueries) {
					System.out.println(
						"QConJoin evaluatePending FALSE "
							+ i_id
							+ " Calling: "
							+ i_constraint1.i_id
							+ ", "
							+ i_constraint2.i_id);
				}
				i_constraint1.doNotInclude(a_root);
				i_constraint2.doNotInclude(a_root);
			}else{
				if (Deploy.debugQueries) {
					System.out.println(
						"QConJoin evaluatePending TRUE "
							+ i_id
							+ " NOT calling: "
							+ i_constraint1.i_id
							+ ", "
							+ i_constraint2.i_id);
				}
			}

		}
	}

	QCon getOtherConstraint(QCon a_constraint) {
		if (a_constraint == i_constraint1) {
			return i_constraint2;
		} else if (a_constraint == i_constraint2) {
			return i_constraint1;
		}
		if (Deploy.debug) {
			throw new RuntimeException("Should never happen.");
		}
		return null;
	}
	
	String logObject(){
		if (Deploy.debugQueries) {
			String msg = i_and ? "&" : "|";
			return " " + i_constraint1.i_id + msg + i_constraint2.i_id;
		}else{
			return "";
		}
	}
	
	
	boolean removeForParent(QCon a_constraint) {
		if (i_and) {
			QCon other = getOtherConstraint(a_constraint);
			other.removeJoin(this); // prevents circular call
			other.remove();
			return true;
		}
		return false;
	}
	
	public String toString(){
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
		String str = "QConJoin " + (i_and ? "AND ": "OR");
		if(i_constraint1 != null){
			str += "\n   " + i_constraint1;  
		}
		if(i_constraint2 != null){
			str += "\n   " + i_constraint2;  
		}
		return str;
	}
	

}
