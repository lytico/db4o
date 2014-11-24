package com.db4o.omplus.connection;

import java.io.*;
import java.net.*;
import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.foundation.*;

public abstract class ConnectionParams {
	
	protected String[] jarPaths;
	protected String[] configNames;
	
	protected boolean allowUpdates = false;
	
	public ConnectionParams(String[] jarPaths, String[] configNames) {
		this.jarPaths = jarPaths;
		this.configNames = configNames;
	}
	public abstract String getPath();
	public abstract ObjectContainer connect(Function4<String, Boolean> userCallback) throws DBConnectException;
	
	public final ObjectContainer connect() throws DBConnectException {
		return connect(new Function4<String, Boolean>() {
			public Boolean apply(String arg0) {
				return false;
			}
		});
	}
	
//	TODO: should get the activation and update depth from preferences
	protected void configureCommon(CommonConfiguration config){
		config.allowVersionUpdates(allowUpdates);
		config.activationDepth(0);
// see OMJ-154
//		config.add(new DotnetSupport(true));
	}
	
	public int jarPathCount() {
		return jarPaths.length;
	}
	
	public String[] jarPaths() {
		return Arrays.copyOf(jarPaths, jarPaths.length);
	}

	public int configuratorClassNameCount() {
		return configNames.length;
	}
	
	public String[] configuratorClassNames() {
		return Arrays.copyOf(configNames, configNames.length);
	}
	
	public void jarPaths(String[] jarPaths) {
		this.jarPaths = jarPaths == null ? new String[0] : jarPaths;
	}
	
	public void configuratorClassNames(String[] configNames) {
		this.configNames = configNames == null ? new String[0] : configNames;
	}
	
	public void configureUpdates() {
		allowUpdates = true;
	}
	
	public String toString() {
		return getPath();
	}

	protected URL[] jarURLs() throws DBConnectException {
		if(jarPaths == null || jarPaths.length == 0) {
			return new URL[0];
		}
		URL[] urls = new URL[jarPaths.length];
		for (int jarIdx = 0; jarIdx < jarPaths.length; jarIdx++) {
			File jarFile = new File(jarPaths[jarIdx]);
			if(!jarFile.isFile() || !jarFile.canRead()) {
				throw new DBConnectException(this, "cannot access jar: " + jarFile.getAbsolutePath());
			}
			try {
				urls[jarIdx] = jarFile.toURI().toURL();
			} 
			catch (MalformedURLException exc) {
				throw new DBConnectException(this, "invalid url for jar: " + jarFile.getAbsolutePath(), exc);
			}
		}
		return urls;
	}
}
