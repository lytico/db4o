/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.omplus.datalayer;

import java.util.*;

import org.eclipse.core.runtime.*;

import com.db4o.*;
import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.propertyViewer.*;

public class DatabaseModel {
	private IDbInterface dbi;
	private PropertiesManager props;
	private Set<DatabaseModelListener> listeners = new HashSet<DatabaseModelListener>();
	
	public IDbInterface db() {
		return dbi;
	}
	
	public PropertiesManager props() {
		return props;
	}
	
	public void connect(ObjectContainer db, String path) {
		disconnect(false);
		dbi = new DbInterfaceImpl(db, path);
		props = new PropertiesManager(dbi);
		notifyListeners();
	}
	
	public boolean connected() {
		return dbi != null;
	}

	public void disconnect() {
		disconnect(true);
	}

	private void disconnect(boolean notifyListeners) {
		if(dbi == null) {
			return;
		}
		try {
			dbi.close();
		}
		finally {
			dbi = null;
			props = null;
			if(notifyListeners) {
				notifyListeners();
			}
		}
	}

	public void registerListener(DatabaseModelListener listener) {
		listeners.add(listener);
	}

	public void unregisterListener(DatabaseModelListener listener) {
		listeners.remove(listener);
	}

	private void notifyListeners() {
		for (DatabaseModelListener  listener : listeners) {
			try {
				listener.connectedStatusChangedTo(connected());
			}
			catch(Exception exc) {
				IStatus status = new Status(IStatus.ERROR, Activator.PLUGIN_ID, "error during listener notification", exc);
				Activator.getDefault().getLog().log(status);
			}
		}
	}
}
