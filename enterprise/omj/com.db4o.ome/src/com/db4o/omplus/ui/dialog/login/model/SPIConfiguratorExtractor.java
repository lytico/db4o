/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.model;

import java.io.*;
import java.net.*;
import java.util.*;

import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;

public class SPIConfiguratorExtractor<S> implements ConfiguratorExtractor {

	private final Class<S> spi;

	public SPIConfiguratorExtractor(Class<S> spi) {
		this.spi = spi;
	}
	
	public List<String> configuratorClassNames(File jarFile) throws DBConnectException {
		URLClassLoader cl = new URLClassLoader(new URL[] { url(jarFile) }, Activator.class.getClassLoader());
		Iterator<S> ps = SunSPIUtil.retrieveSPIImplementors(spi, cl);
		Set<String> classNames = new HashSet<String>();
		while(ps.hasNext()) {
			Object configurator = ps.next();
			if(configurator == null || !spi.isAssignableFrom(configurator.getClass())) {
				throw new DBConnectException("retrieved invalid configurator: " + configurator);
			}
			classNames.add(configurator.getClass().getName());
		}
		List<String> configClassNames = new ArrayList<String>(classNames);
		Collections.sort(configClassNames);
		return configClassNames;
	}

	private URL url(File jarFile) throws DBConnectException { // FIXME better exception
		try {
			return jarFile.toURI().toURL();
		} 
		catch (MalformedURLException exc) {
			throw new DBConnectException("invalid jar reference: " + jarFile, exc);
		}
	}

	public boolean acceptJarFile(File file) {
		return file.isFile() && file.getName().endsWith(".jar");
	}
}
