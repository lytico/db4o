package com.db4o.ta.instrumentation.test;

import java.net.*;

import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.main.*;

public class InstrumentationEnvironment {

	public static Class enhance(Class origClazz, BloatClassEdit edit) throws ClassNotFoundException {
		BloatInstrumentingClassLoader loader = new BloatInstrumentingClassLoader(new URL[0], InstrumentationEnvironment.class.getClassLoader(), edit);
		return loader.loadClass(origClazz.getName());
	}
	
}
