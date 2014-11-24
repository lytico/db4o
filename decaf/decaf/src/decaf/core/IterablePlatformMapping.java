/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */
package decaf.core;

import org.eclipse.jdt.core.dom.*;

public interface IterablePlatformMapping {

	public static final IterablePlatformMapping JDK11_ITERABLE_MAPPING = new IterableJdk11Mapping();
	public static final IterablePlatformMapping JDK12_ITERABLE_MAPPING = new IterableJdk12Mapping();
	String iteratorClassName();
	String iteratorNextCheckName();
	String iteratorNextElementName();

	Expression coerceIterableExpression(Expression iterableExpr);
	Expression unwrapIterableExpression(Expression iterableExpr);

}
