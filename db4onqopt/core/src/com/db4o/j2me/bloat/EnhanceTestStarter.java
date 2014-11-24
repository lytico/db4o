/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.j2me.bloat;

import java.io.*;
import java.lang.reflect.*;
import java.net.*;

public class EnhanceTestStarter {
	public static void main(String[] args) throws Exception {
		Generation.main(new String[0]);
		String[] classpath={
				"generated",
				"bin",
				"../db4oj/bin",
				"../db4ojdk1.2/bin",
		};
		URL[] urls=new URL[classpath.length];
		for (int pathIdx = 0; pathIdx < classpath.length; pathIdx++) {
			urls[pathIdx]=new File(classpath[pathIdx]).toURI().toURL();
		}
		// a risky move, but usually this should be the ext classloader
		ClassLoader extCL = ClassLoader.getSystemClassLoader().getParent();
		URLClassLoader urlCL=new URLClassLoader(urls,extCL);
		Class mainClazz=urlCL.loadClass(EnhanceTestMain.class.getName());
		Method mainMethod=mainClazz.getMethod("main",new Class[]{String[].class});
		mainMethod.invoke(null, new Object[]{new String[0]});
	}
}
