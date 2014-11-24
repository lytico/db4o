package com.db4o.instrumentation.classfilter;

import java.util.jar.*;
import java.util.zip.*;

import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.util.*;

public class JarFileClassFilter implements ClassFilter {

	private JarFile _jarFile;
	
	public JarFileClassFilter(JarFile jarFile) {
		_jarFile = jarFile;
	}

	public boolean accept(Class clazz) {
		String path = BloatUtil.classPathForName(clazz.getName());
		ZipEntry entry = _jarFile.getEntry(path);
		return entry != null;
	}
}
