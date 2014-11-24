/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.cmp.operand;

import com.db4o.instrumentation.api.*;


public class FieldValue extends ComparisonOperandDescendant {
	
	private final FieldRef _field;

	public FieldValue(ComparisonOperandAnchor root, FieldRef field) {
		super(root);
		_field = field;
	}

	public String fieldName() {
		return _field.name();
	}
	
	public boolean equals(Object other) {
		if(!super.equals(other)) {
			return false;
		}
		FieldValue casted = (FieldValue) other;
		return _field.equals(casted._field);
	}
	
	public int hashCode() {
		return super.hashCode()*29+_field.hashCode();
	}
	
	public String toString() {
		return super.toString()+"."+_field;
	}
	
	public void accept(ComparisonOperandVisitor visitor) {
		visitor.visit(this);
	}
	
	/**
	 * @sharpen.property
	 */
	public FieldRef field() {
		return _field;
	}

	/**
	 * @sharpen.property
	 */
	@Override
	public TypeRef type() {
		return _field.type();
	}
}
