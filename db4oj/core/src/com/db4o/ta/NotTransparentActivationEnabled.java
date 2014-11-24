/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.ta;

import com.db4o.diagnostic.*;
import com.db4o.internal.*;

public class NotTransparentActivationEnabled extends DiagnosticBase {

	private ClassMetadata _class;
	
	public NotTransparentActivationEnabled(ClassMetadata clazz) {
		_class = clazz;
	}

	public String problem() {
		return "An object of class "+_class+" was stored. Instances of this class very likely are not subject to transparent activation.";
	}

	public Object reason() {
		return _class;
	}

	public String solution() {
		return "Use a TA aware class with equivalent functionality or ensure that this class provides a sensible implementation of the " + Activatable.class.getName() + " interface and the implicit TA hooks, either manually or by applying instrumentation.";
	}
}
