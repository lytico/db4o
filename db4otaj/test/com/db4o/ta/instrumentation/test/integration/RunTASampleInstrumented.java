/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.integration;

import java.lang.reflect.*;
import java.net.*;

import com.db4o.instrumentation.*;
import com.db4o.instrumentation.classfilter.*;
import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.main.*;
import com.db4o.ta.instrumentation.*;

public class RunTASampleInstrumented {

	public static void main(String[] args) throws Exception {
		ClassFilter filter = new ByNameClassFilter(new String[] { Project.class.getName(), PrioritizedProject.class.getName(), UnitOfWork.class.getName() });
		BloatClassEdit edit = new InjectTransparentActivationEdit(filter);
		ClassLoader loader = new BloatInstrumentingClassLoader(new URL[]{}, RunTASampleInstrumented.class.getClassLoader(), filter, edit);
		Class mainClass = loader.loadClass(TransparentActivationSampleMain.class.getName());
		Method mainMethod = mainClass.getMethod("main", new Class[]{ String[].class });
		mainMethod.invoke(null, new Object[]{ new String[]{} });
	}
}
