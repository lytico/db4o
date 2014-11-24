/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query;

import com.db4o.query.*;

/**
 * @exclude
 */
public class PredicateEvaluation implements Evaluation {
    
    public Predicate _predicate;
    
    public PredicateEvaluation(){
        // CS
    }
    
    public PredicateEvaluation(Predicate predicate){
        _predicate = predicate;
    }

    public void evaluate(Candidate candidate) {
        candidate.include(_predicate.appliesTo(candidate.getObject()));
    }

}
