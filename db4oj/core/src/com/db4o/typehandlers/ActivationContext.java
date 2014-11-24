package com.db4o.typehandlers;

import com.db4o.internal.*;
import com.db4o.internal.activation.*;
import com.db4o.marshall.*;

public interface ActivationContext extends Context {

	void cascadeActivationToTarget();

	void cascadeActivationToChild(Object obj);

	ObjectContainerBase container();

	Object targetObject();

	ClassMetadata classMetadata();

	ActivationDepth depth();

	ActivationContext forObject(Object newTargetObject);

	ActivationContext descend();

}