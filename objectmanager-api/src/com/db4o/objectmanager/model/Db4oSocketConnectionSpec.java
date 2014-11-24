package com.db4o.objectmanager.model;

import java.util.logging.*;

public class Db4oSocketConnectionSpec extends Db4oConnectionSpec {

    private static Logger logger = Logger.getLogger(Db4oConnectionSpec.class.getName());

    private String host;
	private int port;
	private String user;
	private String password;

	public Db4oSocketConnectionSpec(String host,int port,String user,String password,boolean readOnly) {
		super(readOnly);
		this.host=host;
		this.port=port;
		this.user=user;
		this.password=password;
	}

	public String getPath() {
		return "db4o://" + host + ":" + port;
	}

	/*protected ObjectContainer connectInternal() {
		try {
			return Db4o.openClient(host, port, user, password);
		} catch (IOException exc) {
			logger.log(Level.SEVERE, "Could not connect to: "+ getPath()+" as user "+user, exc);
			return null;
		}
	}*/

	public boolean isRemote() {
		return true;
	}

	public String toString() {
        return getPath();
    }

    public String getFullPath() {
		return getPath();
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

    public String getPassword() {
        return password;
    }
}
