/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery.expr.cmp.operand;


import com.db4o.instrumentation.api.*;


public class ArrayAccessValue extends ComparisonOperandDescendant {
	private ComparisonOperand _index;
	
	public ArrayAccessValue(ComparisonOperandDescendant parent,ComparisonOperand index) {
		super(parent);
		_index = index;
	}

	public void accept(ComparisonOperandVisitor visitor) {
		visitor.visit(this);
	}
	
	public ComparisonOperand index() {
		return _index;
	}
	
	public boolean equals(Object obj) {
		if(!super.equals(obj)) {
			return false;
		}
		ArrayAccessValue casted=(ArrayAccessValue)obj;
		return _index.equals(casted._index);
	}
	
	public int hashCode() {
		return super.hashCode()*29+_index.hashCode();
	}
	
	public String toString() {
		return super.toString()+"["+_index+"]";
	}

	/**
	 * @sharpen.property
	 */
	@Override
	public TypeRef type() {
		return ((ComparisonOperandDescendant)parent()).type().elementType();
	}
}
