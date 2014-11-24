/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.nativequery.analysis;

import java.util.*;

import EDU.purdue.cs.bloat.tree.*;

import com.db4o.nativequery.expr.*;
import com.db4o.nativequery.expr.build.*;
import com.db4o.nativequery.expr.cmp.*;
import com.db4o.nativequery.expr.cmp.operand.*;

public class ComparisonExpressionFactory {

	private final ExpressionBuilder _expBuilder;
	private final Map _builders = new HashMap();

	public ComparisonExpressionFactory(ExpressionBuilder expBuilder) {
		_expBuilder = expBuilder;
		_builders.put(new BuilderSpec(IfStmt.EQ,false), new PlainComparisonBuilder(
				ComparisonOperator.REFERENCE_EQUALITY));
		_builders.put(new BuilderSpec(IfStmt.EQ,true), new PlainComparisonBuilder(
				ComparisonOperator.VALUE_EQUALITY));
		_builders.put(new BuilderSpec(IfStmt.NE,false), new NegateComparisonBuilder(
				ComparisonOperator.REFERENCE_EQUALITY));
		_builders.put(new BuilderSpec(IfStmt.NE,true), new NegateComparisonBuilder(
				ComparisonOperator.VALUE_EQUALITY));
		_builders.put(new BuilderSpec(IfStmt.LT,false), new PlainComparisonBuilder(
				ComparisonOperator.SMALLER));
		_builders.put(new BuilderSpec(IfStmt.LT,true),builder(IfStmt.LT,false));
		_builders.put(new BuilderSpec(IfStmt.GT,false), new PlainComparisonBuilder(
				ComparisonOperator.GREATER));
		_builders.put(new BuilderSpec(IfStmt.GT,true),builder(IfStmt.GT,false));
		_builders.put(new BuilderSpec(IfStmt.LE,false), new NegateComparisonBuilder(
				ComparisonOperator.GREATER));
		_builders.put(new BuilderSpec(IfStmt.LE,true),builder(IfStmt.LE,false));
		_builders.put(new BuilderSpec(IfStmt.GE,false), new NegateComparisonBuilder(
				ComparisonOperator.SMALLER));
		_builders.put(new BuilderSpec(IfStmt.GE,true),builder(IfStmt.GE,false));
	}

	public Expression buildComparison(int op, boolean isPrimitive, FieldValue fieldExpr, ComparisonOperand valueExpr) {
		return builder(op,isPrimitive).buildComparison(fieldExpr, valueExpr);
	}
	
    private PlainComparisonBuilder builder(int op, boolean primitive) {
		return (PlainComparisonBuilder) _builders.get(new BuilderSpec(op,primitive));
	}

	private static class BuilderSpec {
		private int _op;
		private boolean _primitive;

		public BuilderSpec(int op, boolean primitive) {
			this._op = op;
			this._primitive = primitive;
		}

		public int hashCode() {
			final int prime = 31;
			int result = 1;
			result = prime * result + _op;
			result = prime * result + (_primitive ? 1231 : 1237);
			return result;
		}

		public boolean equals(Object obj) {
			if (this == obj)
				return true;
			if (obj == null)
				return false;
			if (getClass() != obj.getClass())
				return false;
			final BuilderSpec other = (BuilderSpec) obj;
			if (_op != other._op)
				return false;
			if (_primitive != other._primitive)
				return false;
			return true;
		}		
	}
	
	private static class PlainComparisonBuilder {
		private ComparisonOperator op;

		public PlainComparisonBuilder(ComparisonOperator op) {
			this.op = op;
		}

		public Expression buildComparison(FieldValue fieldValue, ComparisonOperand valueExpr) {
			if (TypeRefUtil.isBooleanField(fieldValue)) {
				if (valueExpr instanceof ConstValue) {
					ConstValue constValue = (ConstValue) valueExpr;
					if (constValue.value() instanceof Integer) {
						Integer intValue = (Integer) constValue.value();
						Boolean boolValue = (intValue.intValue() == 0 ? Boolean.FALSE
								: Boolean.TRUE);
						valueExpr = new ConstValue(boolValue);
					}
				}
			}
			return new ComparisonExpression(fieldValue, valueExpr, op);
		}
	}

	private class NegateComparisonBuilder extends PlainComparisonBuilder {
		public NegateComparisonBuilder(ComparisonOperator op) {
			super(op);
		}

		public Expression buildComparison(FieldValue fieldValue, ComparisonOperand valueExpr) {
			return _expBuilder.not(super.buildComparison(fieldValue, valueExpr));
		}
	}

}
