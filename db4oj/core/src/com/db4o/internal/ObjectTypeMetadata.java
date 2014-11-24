package com.db4o.internal;

import com.db4o.internal.marshall.*;
import com.db4o.reflect.*;
import com.db4o.typehandlers.*;

public class ObjectTypeMetadata extends PrimitiveTypeMetadata {

	public ObjectTypeMetadata(ObjectContainerBase container,
			TypeHandler4 handler, int id, ReflectClass classReflector) {
		super(container, handler, id, classReflector);
	}
	
	@Override
	public Object instantiate(UnmarshallingContext context) {
		final Object object = new Object();
		onInstantiate(context, object);
		return object;
	}

}
