/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.api.prefs;

import java.util.LinkedList;
import java.util.ListIterator;



public class ClasspathPreferences {

    public static final String CLASSPATH_PREFERENCES_ID = "Classpath Preferences";
    
    private static ClasspathPreferences classpathPrefs = null;

    public static ClasspathPreferences getDefault() {
        // If the object already exists in the preference store, just return
        // it. Otherwise, create it.
        ClasspathPreferences result = (ClasspathPreferences) Preferences
                .getDefault().getPreference(CLASSPATH_PREFERENCES_ID);

        if (result == null) {
            result = new ClasspathPreferences();
        }
        return result;
    }
    
    private LinkedList entries = null;

    private LinkedList entries() {
        if (entries == null) {
            entries = new LinkedList();
        }
        return entries;
    }
    
    public void resetDefaultValues() {
        entries().clear();
    }
    
    public ListIterator iterator() {
        return entries().listIterator();
    }
    
    public void add(String entry) {
        entries().addLast(entry);
    }
    
    public void remove(String entry) {
        entries().remove(entry);
    }

	public String[] classPath() {
		return (String[])entries().toArray(new String[entries().size()]);
	}

}
