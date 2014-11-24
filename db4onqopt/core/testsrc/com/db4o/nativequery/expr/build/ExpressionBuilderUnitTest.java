/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.build;

import com.db4o.nativequery.expr.*;
import com.db4o.nativequery.expr.cmp.*;
import com.db4o.nativequery.expr.cmp.field.*;

import db4ounit.*;

public class ExpressionBuilderUnitTest implements TestCase, TestLifeCycle {
	private MockComparisonExpressionBuilder mockBuilder;
	private ExpressionBuilder builder;
	private Expression expr;
	private Expression other;
	
	public void setUp() throws Exception {
		mockBuilder=new MockComparisonExpressionBuilder();
		builder=new ExpressionBuilder();
		expr=mockBuilder.build();
		other=mockBuilder.build();
	}
	
	public void testConstant() {
		Assert.areSame(BoolConstExpression.TRUE,builder.constant(Boolean.TRUE));
		Assert.areSame(BoolConstExpression.FALSE,builder.constant(Boolean.FALSE));
		// TODO: Move to const expr (or expr) test
		Assert.areEqual(BoolConstExpression.FALSE,BoolConstExpression.expr(false));
		Assert.areEqual(BoolConstExpression.TRUE,BoolConstExpression.expr(true));
	}

	public void testNot() {
		Assert.areSame(BoolConstExpression.FALSE,builder.not(BoolConstExpression.TRUE));
		Assert.areSame(BoolConstExpression.TRUE,builder.not(BoolConstExpression.FALSE));
		Assert.areSame(BoolConstExpression.TRUE,builder.not(builder.not(BoolConstExpression.TRUE)));
		Assert.areSame(BoolConstExpression.FALSE,builder.not(builder.not(BoolConstExpression.FALSE)));
		Assert.areEqual(new NotExpression(expr),builder.not(expr));
		Assert.areEqual(new ComparisonExpression(new FieldValue(CandidateFieldRoot.INSTANCE,"foo"),new ConstValue(Boolean.TRUE),ComparisonOperator.EQUALS),
					builder.not(new ComparisonExpression(new FieldValue(CandidateFieldRoot.INSTANCE,"foo"),new ConstValue(Boolean.FALSE),ComparisonOperator.EQUALS)));
	}
	
	public void testAnd() {
		Assert.areSame(BoolConstExpression.FALSE,builder.and(BoolConstExpression.FALSE,expr));
		Assert.areSame(BoolConstExpression.FALSE,builder.and(expr,BoolConstExpression.FALSE));
		Assert.areSame(expr,builder.and(BoolConstExpression.TRUE,expr));
		Assert.areSame(expr,builder.and(expr,BoolConstExpression.TRUE));
		Assert.areEqual(expr,builder.and(expr,expr));
		Assert.areEqual(BoolConstExpression.FALSE,builder.and(expr,builder.not(expr)));
		Assert.areEqual(new AndExpression(expr,other),builder.and(expr,other));
	}

	public void testOr() {
		Assert.areSame(BoolConstExpression.TRUE,builder.or(BoolConstExpression.TRUE,expr));
		Assert.areSame(BoolConstExpression.TRUE,builder.or(expr,BoolConstExpression.TRUE));
		Assert.areSame(expr,builder.or(BoolConstExpression.FALSE,expr));
		Assert.areSame(expr,builder.or(expr,BoolConstExpression.FALSE));
		Assert.areSame(expr,builder.or(expr,expr));
		Assert.areEqual(BoolConstExpression.TRUE,builder.or(expr,builder.not(expr)));
		Assert.areEqual(new OrExpression(expr,other),builder.or(expr,other));
	}
	
	public void testIfThenElse() {
		Assert.areSame(expr,builder.ifThenElse(BoolConstExpression.TRUE,expr,other));
		Assert.areSame(other,builder.ifThenElse(BoolConstExpression.FALSE,expr,other));
		Assert.areSame(BoolConstExpression.TRUE,builder.ifThenElse(expr,BoolConstExpression.TRUE,BoolConstExpression.TRUE));
		Assert.areSame(BoolConstExpression.FALSE,builder.ifThenElse(expr,BoolConstExpression.FALSE,BoolConstExpression.FALSE));
		Assert.areSame(expr,builder.ifThenElse(expr,BoolConstExpression.TRUE,BoolConstExpression.FALSE));
		Assert.areEqual(new NotExpression(expr),builder.ifThenElse(expr,BoolConstExpression.FALSE,BoolConstExpression.TRUE));
		Assert.areEqual(builder.or(expr,other),builder.ifThenElse(expr,BoolConstExpression.TRUE,other));
		// FIXME more compund boolean constraint tests
		//Assert.areEqual(builder.or(expr,builder.and(builder.not(expr),other)),builder.ifThenElse(expr,BoolConstExpression.TRUE,other));
	}
	
	public void testCombined() {
		Expression a=mockBuilder.build();
		Expression b=mockBuilder.build();
		Expression exp1=builder.and(a,builder.constant(Boolean.TRUE));
		Expression exp2=builder.and(BoolConstExpression.FALSE,builder.not(b));
		Expression exp=builder.or(exp1,exp2);
		Assert.areEqual(a,exp);
	}

	public void tearDown() throws Exception {
	}
}
