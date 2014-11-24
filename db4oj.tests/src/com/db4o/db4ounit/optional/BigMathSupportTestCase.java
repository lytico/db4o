/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.optional;

import static org.easymock.EasyMock.createMock;
import static org.easymock.EasyMock.isA;
import static org.easymock.EasyMock.replay;
import static org.easymock.EasyMock.verify;

import java.math.*;

import org.easymock.*;

import com.db4o.config.*;
import com.db4o.internal.*;
import com.db4o.internal.handlers.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.db4o.typehandlers.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class BigMathSupportTestCase implements TestCase {
	
	private final class TypeHandlerPredicateMatcher implements IArgumentMatcher {
	    private final Class<?> acceptedClass;
	    private final Class<?> rejectedClass;

	    private TypeHandlerPredicateMatcher(Class<?> acceptedClass, Class<?> rejectedClass) {
		    this.acceptedClass = acceptedClass;
		    this.rejectedClass = rejectedClass;
	    }

	    public void appendTo(StringBuffer buffer) {
	    	buffer.append("TypeHandlerPredicate { Accepted: ");
	    	buffer.append(acceptedClass);
	    	buffer.append(", Rejected: ");
	    	buffer.append(rejectedClass);
	    	buffer.append(" }");
	    }

	    public boolean matches(Object arg) {
	    	TypeHandlerPredicate predicate = (TypeHandlerPredicate) arg;
	    	return predicate.match(reflectClassFor(acceptedClass))
	    		&& !predicate.match(reflectClassFor(rejectedClass));
	    }
    }

	final Reflector reflector = new GenericReflector(Platform4.reflectorForType(getClass()));
	
	public void testPrepare() {
		final Configuration configuration = createMock(Configuration.class);
		
		configuration.registerTypeHandler(
				eqTypeHandlerPredicate(BigInteger.class, BigDecimal.class),
				isA(BigIntegerTypeHandler.class));
		
		configuration.registerTypeHandler(
				eqTypeHandlerPredicate(BigDecimal.class, BigInteger.class),
				isA(BigDecimalTypeHandler.class));
		
		replay(configuration);
		
		new BigMathSupport().prepare(configuration);
		
		verify(configuration);
	}

	private TypeHandlerPredicate eqTypeHandlerPredicate(final Class<?> acceptedClass,
            final Class<?> rejectedClass) {
	    EasyMock.reportMatcher(new TypeHandlerPredicateMatcher(acceptedClass, rejectedClass));
		return null;
    }
	
	private ReflectClass reflectClassFor(final Class<?> clazz) {
	    return reflector.forClass(clazz);
    }

}
