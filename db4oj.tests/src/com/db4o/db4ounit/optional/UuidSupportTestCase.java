/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.optional;

import static org.easymock.EasyMock.*;

import java.util.*;

import org.easymock.*;

import com.db4o.config.*;
import com.db4o.internal.*;
import com.db4o.internal.handlers.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.db4o.typehandlers.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class UuidSupportTestCase implements TestCase {
	
	private final class TypeHandlerPredicateMatcher implements IArgumentMatcher {
	    private final Class<?> acceptedClass;

	    private TypeHandlerPredicateMatcher(Class<?> acceptedClass) {
		    this.acceptedClass = acceptedClass;
	    }

	    public void appendTo(StringBuffer buffer) {
	    	buffer.append("TypeHandlerPredicate { Accepted: ");
	    	buffer.append(acceptedClass);
	    	buffer.append(" }");
	    }

	    public boolean matches(Object arg) {
	    	TypeHandlerPredicate predicate = (TypeHandlerPredicate) arg;
	    	return predicate.match(reflectClassFor(acceptedClass));
	    }
    }

	final Reflector reflector = new GenericReflector(Platform4.reflectorForType(getClass()));
	
	public void testPrepare() {
		final Configuration configuration = createMock(Configuration.class);
		
		configuration.registerTypeHandler(
				eqTypeHandlerPredicate(UUID.class),
				isA(UuidTypeHandler.class));
		
		replay(configuration);
		
		new UuidSupport().prepare(configuration);
		
		verify(configuration);
	}

	private TypeHandlerPredicate eqTypeHandlerPredicate(final Class<?> acceptedClass) {
	    EasyMock.reportMatcher(new TypeHandlerPredicateMatcher(acceptedClass));
		return null;
    }
	
	private ReflectClass reflectClassFor(final Class<?> clazz) {
	    return reflector.forClass(clazz);
    }

}
