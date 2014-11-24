/*
 * SodaEvaluation.java
 *
 * Created on 09 May 2006, 22:03
 *
 */

package com.db4o.test;

import com.db4o.query.Candidate;
import com.db4o.query.Evaluation;

/**
 *
 * @author Larysa
 */
public class SodaEvaluation implements Evaluation{
    public void evaluate(Candidate candidate) {
        Dog dog=(Dog) candidate.getObject();
        candidate.include(dog._parents.length==0);
    }
    
}
