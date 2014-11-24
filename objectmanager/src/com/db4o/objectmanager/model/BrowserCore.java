/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model;

import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedList;

import com.db4o.browser.gui.standalone.ICloseListener;
import com.db4o.browser.prefs.PreferencesCore;

/**
 * BrowserCore.  The root of the model hierarchy in the browser.
 *
 * @author djo
 */
public class BrowserCore implements ICloseListener {
    private static BrowserCore model = null;
    
	public static BrowserCore getDefault() {
        if (model == null) {
            model = new BrowserCore();
			PreferencesCore.initialize();
        }
        return model;
    }
	
	private LinkedList databases = new LinkedList();
    private HashMap dbMap = new HashMap();  // Maps path/filename to database
    
	public IDatabase getDatabase(Db4oConnectionSpec spec) {
		IDatabase requested = (IDatabase) dbMap.get(spec.path());
		if (requested == null) {
            requested = new Db4oDatabase();
            requested.open(spec);
            dbMap.put(spec.path(), requested);
            databases.addLast(requested);
        }
		return requested;
	}

	/**
	 * Gets an array of all open databases
	 * 
	 * @return Database[] all open databases
	 */
	public IDatabase[] getAllDatabases() {
		return (IDatabase[]) dbMap.values().toArray(new IDatabase[dbMap.size()]);
	}

    /* (non-Javadoc)
	 * @see com.db4o.browser.gui.standalone.ICloseListener#closing()
	 */
	public void closing() {
		closeAllDatabases();
		PreferencesCore.close();
	}

	/**
	 * Close all open databases and forget that they ever existed.
	 */
	public void closeAllDatabases() {
		for (Iterator i = databases.iterator(); i.hasNext();) {
			IDatabase database = (IDatabase) i.next();
			database.closeIfOpen();
		}
		databases.clear();
		dbMap.clear();
	}
    
    /**
     * Method iterator.  Returns an IGraphIterator on the most recently opened
     * database.  Returns null if there is no open database.
     * 
     * @return IGraphIterator an iterator on the current open database; null if
     * no database is open.
     */
    public IGraphIterator iterator() {
        if (databases.isEmpty()) {
            return null;
        }
        IDatabase current = (IDatabase) databases.getLast();
        return current.graphIterator();
    }
    
    public IGraphIterator iterator(Db4oConnectionSpec spec) {
        IDatabase requested = getDatabase(spec);
        return requested.graphIterator();
    }


    /**
     * @return true if at least one database is open; false otherwise.
     */
    public boolean isOpen() {
        return databases.size() > 0;
    }

    public void updateClasspath() {
        for (Iterator databaseIter = databases.iterator(); databaseIter.hasNext();) {
            IDatabase database = (IDatabase) databaseIter.next();
            database.reopen();

        }
        fireClassPathChangedEvent();
    }
    
	public void updateCallConstructorPrefs() {
		fireCloseEditorsEvent();
		closeAllDatabases();
	}

    private LinkedList coreListeners = new LinkedList();
    
    public void addBrowserCoreListener(IBrowserCoreListener listener) {
        coreListeners.add(listener);
    }
    
    public void removeBrowserCoreListener(IBrowserCoreListener listener) {
        coreListeners.remove(listener);
    }

    private void fireClassPathChangedEvent() {
        for (Iterator listeners = coreListeners.iterator(); listeners.hasNext();) {
            IBrowserCoreListener listener = (IBrowserCoreListener) listeners.next();
            listener.classpathChanged(this);
        }
    }
    
    private void fireCloseEditorsEvent() {
        for (Iterator listeners = coreListeners.iterator(); listeners.hasNext();) {
            IBrowserCoreListener listener = (IBrowserCoreListener) listeners.next();
            listener.closeEditors(this);
        }
    }

}
