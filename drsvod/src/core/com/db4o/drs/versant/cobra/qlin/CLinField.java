/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.cobra.qlin;

import com.db4o.qlin.*;
import com.versant.odbms.query.*;
import com.versant.odbms.query.Operator.*;

/**
 * TODO: Nodes have to be aggregated and query().setExpression()
 * has to be called in the end.
 * Otherwise or() and and() won't work.
 * CLinConstraint should be used for this task and all constraints
 * should register with the root. 
 */
public class CLinField<T> extends CLinSubNode<T>{
	
	private final Field _field;
	
	public CLinField(CLinRoot<T> root, Object expression) {
		super(root);
		_field = new Field(QLinSupport.field(expression).getName());
	}

	@Override
	public QLin<T> equal(Object obj) {
		Expression expression = new Expression(
				new SubExpression(_field), 
				UnaryOperator.EQUALS, 
				new SubExpression(obj));
		query().setExpression(expression);
		return this;
	}
	
	@Override
	public QLin<T> startsWith(String string) {
		Expression expression = new Expression(
				new SubExpression(_field),
				
				//FIXME: Not sure about this operator. No test case yet.
				UnaryOperator.MATCHES,
				
				new SubExpression(string));
		query().setExpression(expression);
		return this;
	}
	
	@Override
	public QLin<T> smaller(Object obj) {
		Expression expression = new Expression(
				new SubExpression(_field), 
				UnaryOperator.LESS_THAN, 
				new SubExpression(obj));
		query().setExpression(expression);
		return this;
	}
	
	@Override
	public QLin<T> greater(Object obj) {
		Expression expression = new Expression(
				new SubExpression(_field), 
				UnaryOperator.GREATER_THAN, 
				new SubExpression(obj));
		query().setExpression(expression);
		return this;
	}
	


}
