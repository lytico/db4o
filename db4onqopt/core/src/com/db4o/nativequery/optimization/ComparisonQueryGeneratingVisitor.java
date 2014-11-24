/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.nativequery.optimization;

import java.lang.reflect.*;

import com.db4o.instrumentation.api.*;
import com.db4o.internal.Reflection4;
import com.db4o.nativequery.expr.cmp.*;
import com.db4o.nativequery.expr.cmp.operand.*;

final class ComparisonQueryGeneratingVisitor implements ComparisonOperandVisitor {
	private Object _predicate;
	
	private Object _value=null;
	
	private final NativeClassFactory _classSource;

	private final ReferenceResolver _resolver;

	public Object value() {
		return _value;
	}
	
	public void visit(ConstValue operand) {
		_value = operand.value();
	}

	public void visit(FieldValue operand) {
		operand.parent().accept(this);
		Class clazz=((operand.parent() instanceof StaticFieldRoot) ? (Class)_value : _value.getClass());
		try {
			Field field=Reflection4.getField(clazz,operand.fieldName());
			_value=field.get(_value); // arg is ignored for static
		} catch (Exception exc) {
			exc.printStackTrace();
		}
	}

	Object add(Object a,Object b) {
		if(a instanceof Double||b instanceof Double) {
			return new Double(((Double)a).doubleValue()+ ((Double)b).doubleValue());
		}
		if(a instanceof Float||b instanceof Float) {
			return new Float(((Float)a).floatValue()+ ((Float)b).floatValue());
		}
		if(a instanceof Long||b instanceof Long) {
			return new Long(((Long)a).longValue()+ ((Long)b).longValue());
		}
		return new Integer(((Integer)a).intValue()+ ((Integer)b).intValue());
	}

	Object subtract(Object a,Object b) {
		if(a instanceof Double||b instanceof Double) {
	        return new Double(((Double)a).doubleValue()- ((Double)b).doubleValue());
		}
		if(a instanceof Float||b instanceof Float) {
	        return new Float(((Float)a).floatValue() - ((Float)b).floatValue());
		}
		if(a instanceof Long||b instanceof Long) {
	        return new Long(((Long)a).longValue() - ((Long)b).longValue());
		}
	    return new Integer(((Integer)a).intValue() - ((Integer)b).intValue());
	}

	Object multiply(Object a,Object b) {
	    if(a instanceof Double||b instanceof Double) {
	        return new Double(((Double)a).doubleValue() * ((Double)b).doubleValue());
	    }
	    if(a instanceof Float||b instanceof Float) {
	        return new Float(((Float)a).floatValue() * ((Float)b).floatValue());
	    }
	    if(a instanceof Long||b instanceof Long) {
	        return new Long(((Long)a).longValue() * ((Long)b).longValue());
	    }
	    return new Integer(((Integer)a).intValue() * ((Integer)b).intValue());
	}

	Object divide(Object a,Object b) {
	    if(a instanceof Double||b instanceof Double) {
	        return new Double(((Double)a).doubleValue()/ ((Double)b).doubleValue());
	    }
	    if(a instanceof Float||b instanceof Float) {
	        return new Float(((Float)a).floatValue() / ((Float)b).floatValue());
	    }
	    if(a instanceof Long||b instanceof Long) {
	        return new Long(((Long)a).longValue() / ((Long)b).longValue());
	    }
	    return new Integer(((Integer)a).intValue() / ((Integer)b).intValue());
	}

	Object modulo(Object a,Object b) {
	    if(a instanceof Double||b instanceof Double) {
	        return new Double(((Double)a).doubleValue() % ((Double)b).doubleValue());
	    }
	    if(a instanceof Float||b instanceof Float) {
	        return new Float(((Float)a).floatValue() % ((Float)b).floatValue());
	    }
	    if(a instanceof Long||b instanceof Long) {
	        return new Long(((Long)a).longValue() % ((Long)b).longValue());
	    }
	    return new Integer(((Integer)a).intValue() % ((Integer)b).intValue());
	}

	public void visit(ArithmeticExpression operand) {
		operand.left().accept(this);
		Object left=_value;
		operand.right().accept(this);
		Object right=_value;
		switch(operand.op().id()) {
			case ArithmeticOperator.ADD_ID: 
				_value=add(left,right);
				break;
			case ArithmeticOperator.SUBTRACT_ID: 
				_value=subtract(left,right);
				break;
			case ArithmeticOperator.MULTIPLY_ID: 
				_value=multiply(left,right);
				break;
			case ArithmeticOperator.DIVIDE_ID: 
				_value=divide(left,right);
				break;
			case ArithmeticOperator.MODULO_ID: 
				_value=modulo(left,right);
				break;
		}
	}

	public void visit(CandidateFieldRoot root) {
	}

	public void visit(PredicateFieldRoot root) {
		_value=_predicate;
	}

	public void visit(StaticFieldRoot root) {
		try {
			_value=_classSource.forName(root.type().name());
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		}
	}

	public void visit(ArrayAccessValue operand) {
		operand.parent().accept(this);
		Object parent=_value;
		operand.index().accept(this);
		Integer index=(Integer)_value;
		_value=Array.get(parent, index.intValue());
	}

	public void visit(MethodCallValue operand) {
		operand.parent().accept(this);
		Object receiver=_value;
		Method method=_resolver.resolve(operand.method());
		try {
			method.setAccessible(true);
			_value=method.invoke(isStatic(method) ? null : receiver, args(operand));
		} catch (Exception exc) {
			exc.printStackTrace();
			_value=null;
		}
	}

	private Object[] args(MethodCallValue operand) {
		final ComparisonOperand[] args = operand.args();
		Object[] params=new Object[args.length];
		for (int paramIdx = 0; paramIdx < args.length; paramIdx++) {
			args[paramIdx].accept(this);
			params[paramIdx]=_value;
		}
		return params;
	}

	private boolean isStatic(Method method) {
		return NativeQueriesPlatform.isStatic(method);
	}

	public ComparisonQueryGeneratingVisitor(Object predicate, NativeClassFactory classSource, ReferenceResolver resolver) {
		super();
		_predicate = predicate;
		_classSource = classSource;
		_resolver = resolver;
	}
	
}