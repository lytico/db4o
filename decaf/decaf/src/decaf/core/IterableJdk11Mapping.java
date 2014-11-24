/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */
package decaf.core;

import org.eclipse.jdt.core.dom.*;

public class IterableJdk11Mapping implements IterablePlatformMapping {
	
	public String iteratorClassName() {
		return "com.db4o.foundation.Iterator4";
	}

	public String iteratorNextCheckName() {
		return "moveNext";
	}

	public String iteratorNextElementName() {
		return "current";
	}

	public Expression coerceIterableExpression(Expression iterableExpr) {
		return iterableExpr;
	}

	public Expression unwrapIterableExpression(Expression iterableExpr) {
		return iterableExpr;
	}
	
}