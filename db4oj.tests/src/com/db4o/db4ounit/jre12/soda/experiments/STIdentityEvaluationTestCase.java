/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda.experiments;
import com.db4o.*;
import com.db4o.query.*;



/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STIdentityEvaluationTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
    
    public Object[] createData() {
        
        Helper helperA = new Helper("aaa");
        
        return new Object[] {
            new STIdentityEvaluationTestCase(null),
            new STIdentityEvaluationTestCase(helperA),
            new STIdentityEvaluationTestCase(helperA),
            new STIdentityEvaluationTestCase(helperA),
            new STIdentityEvaluationTestCase(new HelperDerivate("bbb")),
            new STIdentityEvaluationTestCase(new Helper("dod"))
            };
    }
    
    public Helper helper;
    
    public STIdentityEvaluationTestCase(){
    }
    
    public STIdentityEvaluationTestCase(Helper h){
        this.helper = h;
    }
    
    public void test(){
        Query q = newQuery();
        
        q.constrain(new Helper("aaa"));
        ObjectSet os = q.execute();
        Helper helperA = (Helper)os.next();
        q = newQuery();
        q.constrain(STIdentityEvaluationTestCase.class);
        q.descend("helper").constrain(helperA).identity();
        q.constrain(new AcceptAllEvaluation());
        expect(q, new int[] {1, 2, 3});
    }
    
    public void testMemberClassConstraint(){
        Query q = newQuery();
        
        q.constrain(STIdentityEvaluationTestCase.class);
        q.descend("helper").constrain(HelperDerivate.class);
        expect(q, new int[] {4});
    }
    
    public static class AcceptAllEvaluation implements Evaluation {
		public void evaluate(Candidate candidate) {
		    candidate.include(true);
		}
	}

	public static class Helper{
        
        public String hString;
        
        public Helper(){
        }
        
        public Helper(String str){
            hString = str;
        }
    }
    
    public static class HelperDerivate extends Helper{
        public HelperDerivate(){
        }
        
        public HelperDerivate(String str){
            super(str);
        }
        
    }
    
}
