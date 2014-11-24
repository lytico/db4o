/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.instrumentation.main;

import java.io.*;
import java.net.*;
import java.util.*;

import EDU.purdue.cs.bloat.context.*;
import EDU.purdue.cs.bloat.editor.*;

import com.db4o.instrumentation.classfilter.*;
import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.util.*;

/**
 * @exclude
 */
public class BloatInstrumentingClassLoader extends BloatingClassLoader {

	private final Map _cache = new HashMap();
	private final ClassFilter _filter;
	private final BloatClassEdit _edit;
	private final BloatLoaderContext _loaderContext = new BloatLoaderContext(getEditorContext());
	private final ClassLoader _filterLoader;
	
	public BloatInstrumentingClassLoader(URL[] urls, ClassLoader parent, BloatClassEdit edit) {
		this(urls, parent, new AcceptAllClassesFilter(), edit);
	}

	public BloatInstrumentingClassLoader(URL[] urls, ClassLoader parent, ClassFilter filter, BloatClassEdit edit) {
		super(urls, parent);
		_filter = filter;
		_edit = edit;
		_filterLoader = new URLClassLoader(urls, parent);
	}

	protected synchronized Class loadClass(String name, boolean resolve) throws ClassNotFoundException {
		if(_cache.containsKey(name)) {
			return (Class)_cache.get(name);
		}
		if(mustDelegate(name)) {
			try {
				Class originalClazz = getParent().loadClass(name);
				return originalClazz;
			}
			catch(ClassNotFoundException exc) {
			}
		}
		Class clazz = shouldLoadRaw(name) ? findRawClass(name) : findClass(name);
		_cache.put(clazz.getName(), clazz);
		if(resolve) {
			resolveClass(clazz);
		}
		return clazz;
	}

	protected boolean mustDelegate(String name) {
		return BloatUtil.isPlatformClassName(name)
				||((name.startsWith("com.db4o.") && name.indexOf("test.")<0 && name.indexOf("samples.")<0));
	}
	
	protected boolean shouldLoadRaw(String name) throws ClassNotFoundException {
		if(mustDelegate(name)) {
			return true;
		}
		Class bogusClazz = _filterLoader.loadClass(name);
		return !_filter.accept(bogusClazz);
	}
	
	private Class findRawClass(String className) throws ClassNotFoundException {
        try {
			String resourcePath = className.replace('.','/') + ".class";
			InputStream resourceStream = getResourceAsStream(resourcePath);
			ByteArrayOutputStream rawByteStream = new ByteArrayOutputStream();
			byte[] buf = new byte[4096];
			int bytesread = 0;
			while((bytesread = resourceStream.read(buf)) >= 0) {
				rawByteStream.write(buf, 0, bytesread);
			}
			resourceStream.close();
			byte[] rawBytes = rawByteStream.toByteArray();
			return super.defineClass(className, rawBytes, 0, rawBytes.length);
		} catch (Exception exc) {
			throw new ClassNotFoundException(className, exc);
		}	
	}

	protected void bloat(ClassEditor ce) {
		_edit.enhance(ce, _filterLoader, _loaderContext);
	}

}
