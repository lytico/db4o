package com.db4o.omplus.connection;

import java.io.*;
import java.net.*;
import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.omplus.*;
import com.db4o.reflect.jdk.*;


public class FileConnectionParams extends ConnectionParams {

	private final static String OLD_FORMAT = "Database is of old Format";
	private final static String GENERIC_OBJ = "com.db4o.reflect.generic.GenericObject";
	private static final String VERSION_UPDATE_TXT = "Old database file format detected. Would you like to upgrade ?";

	private String filePath;
	private boolean readOnly;

	public FileConnectionParams(String path, boolean readOnly, String[] jarPaths, String[] configNames) {
		super(jarPaths, configNames);
		this.filePath = path;
		this.readOnly = readOnly;
	}

	public FileConnectionParams(String path) {
		this(path, false);
	}

	public FileConnectionParams(String path, boolean readOnly) {
		this(path, readOnly, new String[0], new String[0]);
	}

	public String getPath() {
		return filePath;
	}

	public boolean readOnly() {
		return readOnly;
	}

	public void readOnly(boolean readOnly) {
		this.readOnly = readOnly;
	}
	
	public EmbeddedConfiguration configure() throws DBConnectException {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		configureCommon(config.common());
		configureCustom(config);
		config.file().readOnly(readOnly);
		return config;
	}

	@Override
	public ObjectContainer connect(Function4<String, Boolean> userCallback) throws DBConnectException {
		String path = getPath();
		File f = new File(path);
		if (!f.exists() || f.isDirectory()) {
			throw new DBConnectException(this, "File not found: " + f.getAbsolutePath(), new FileNotFoundException(f.getAbsolutePath()));
		}
		try {
			ObjectContainer db = Db4oEmbedded.openFile(configure(), path);
			if (db == null) {
				throw new DBConnectException(this, "Could not open database");
			}
			return db;
		} 
		catch (com.db4o.ext.DatabaseFileLockedException e) {
			throw new DBConnectException(this, "Database is locked by another thread", e);
		} 
		catch (OldFormatException e) {
			throw new DBConnectException(this, OLD_FORMAT, e);
		} 
		catch (IncompatibleFileFormatException e) {
			throw new DBConnectException(this, "Connection closed. Incompatible file format.", e);
		} 
		catch (DatabaseReadOnlyException e) {
			throw new DBConnectException(this, "Database is opened in readonly mode.", e);
		} 
		catch (Db4oException e) {
			throw new DBConnectException(this, "Could not open database.", e);
		}
		// FIXME
		catch (ClassCastException ex) {
			if (ex.getMessage().equals(GENERIC_OBJ))
				throw new DBConnectException(this, "Couldn't open .NET database in OME eclipse plugin", ex);
			throw ex;
		} 
		catch (Exception ex) {
			String message = ex.getMessage();
			if (OLD_FORMAT.equals(message) && userCallback.apply(VERSION_UPDATE_TXT)) {
				configureUpdates();
				return connect();
			}
			throw new DBConnectException(this, "Could not open database.", ex);
		}
	}

	@Override
	public boolean equals(Object other) {
		if(this == other) {
			return true;
		}
		if(other == null || getClass() != other.getClass()) {
			return false;
		}
		FileConnectionParams params = (FileConnectionParams)other;
		return filePath.equals(params.filePath);
	}
	
	@Override
	public int hashCode() {
		return filePath.hashCode();
	}
	
	private void configureCustom(EmbeddedConfiguration config) throws DBConnectException {
		URL[] urls = jarURLs();
		if(urls.length == 0) {
			return;
		}
		URLClassLoader cl = new URLClassLoader(urls, Activator.class.getClassLoader());
		applyConfigurationItems(config, cl);
		config.common().reflectWith(new JdkReflector(cl));
	}

	private void applyConfigurationItems(EmbeddedConfiguration config, URLClassLoader loader) {
		Iterator<EmbeddedConfigurationItem> ps = SunSPIUtil.retrieveSPIImplementors(EmbeddedConfigurationItem.class, loader);
		if(ps.hasNext()) {
			EmbeddedConfigurationItem configurator = ps.next();
			config.addConfigurationItem(configurator);
		}
	}

}
