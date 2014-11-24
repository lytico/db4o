package com.db4o.omplus.connection;

import java.net.*;
import java.util.*;

import com.db4o.*;
import com.db4o.cs.*;
import com.db4o.cs.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.omplus.*;
import com.db4o.reflect.jdk.*;

public class RemoteConnectionParams extends ConnectionParams{

	private String host;
	
	private int port;
	
	private String user;
	
	private transient String password;

	public RemoteConnectionParams(String host,int port,String user,String password) {
		this(host, port, user, password, new String[0], new String[0]);
	}

	public RemoteConnectionParams(String host,int port,String user,String password, String[] jarPaths, String[] configNames) {
		super(jarPaths, configNames);
		this.host=host;
		this.port = port;
		this.user=user;
		this.password=password;
	}

	public String getPath() {
		StringBuilder sb = new StringBuilder();
//		sb.append("db4o://");
		sb.append(host);
		sb.append(":");
		sb.append(port);
		sb.append(":");
		sb.append(user);
		return sb.toString();
	}

	public String getHost() {
		return host;
	}

	public int getPort() {
		return port;
	}

	public String getUser() {
		return user;
	}

	public void user(String user) {
		this.user = user;
	}
	
	public String getPassword() {
		return password == null ? "" : password;
	}
	
	public void password(String password) {
		this.password = password;
	}
	
	public ClientConfiguration configure() throws DBConnectException{
		ClientConfiguration config = Db4oClientServer.newClientConfiguration();
		configureCommon(config.common());
		configureCustom(config);
		return config;
	}

	@Override
	public ObjectContainer connect(Function4<String, Boolean> userCallback) throws DBConnectException {
		try {
			return Db4oClientServer.openClient(configure(), getHost(), getPort(), getUser(), getPassword());
		} 
		catch (InvalidPasswordException e) {
			throw new DBConnectException(this, "Invalid User Credentials", e);
		} 
		catch (Exception e) {
			throw new DBConnectException(this, "Could not connect to remote database", e);
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
		RemoteConnectionParams params = (RemoteConnectionParams) other;
		return host.equals(params.host) && port == params.port;
	}

	@Override
	public int hashCode() {
		return host.hashCode() ^ port;
	}
	
	private void configureCustom(ClientConfiguration config) throws DBConnectException {
		URL[] urls = jarURLs();
		if(urls.length == 0) {
			return;
		}
		URLClassLoader cl = new URLClassLoader(urls, Activator.class.getClassLoader());
		applyConfigurationItems(config, cl);
		config.common().reflectWith(new JdkReflector(cl));
	}

	private void applyConfigurationItems(ClientConfiguration config, URLClassLoader loader) {
		Iterator<ClientConfigurationItem> ps = SunSPIUtil.retrieveSPIImplementors(ClientConfigurationItem.class, loader);
		if(ps.hasNext()) {
			ClientConfigurationItem configurator = ps.next();
			config.addConfigurationItem(configurator);
		}
	}

}
