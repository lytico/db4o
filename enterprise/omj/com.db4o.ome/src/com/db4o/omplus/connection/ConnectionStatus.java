package com.db4o.omplus.connection;

import com.db4o.omplus.*;

public class ConnectionStatus {
	
	public boolean isConnected() {
		return Activator.getDefault().dbModel().connected();
	}
	
	public String getVersion(){
		return Activator.getDefault().dbModel().db().getVersion();
	}
	
	public String getCurrentDB(){
		return Activator.getDefault().dbModel().db().getDbPath();
	}
	
	public void closeExistingDB(){
		Activator.getDefault().dbModel().disconnect();
	}

}
