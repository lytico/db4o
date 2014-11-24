/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.api.prefs;

import java.util.LinkedList;
import java.util.ListIterator;


public class ConstructorPreferences {

    public static final String CONSTRUCTOR_PREFERENCES_ID = "Constructor Preferences";
    
    public static ConstructorPreferences getDefault() {
        // If the object already exists in the preference store, just return
        // it. Otherwise, create it.
        ConstructorPreferences result = (ConstructorPreferences) Preferences
                .getDefault().getPreference(CONSTRUCTOR_PREFERENCES_ID);

        if (result == null) {
            result = new ConstructorPreferences();
        }
        return result;
    }
    
    private LinkedList entries = null;

    private LinkedList entries() {
        if (entries == null) {
            entries = new LinkedList();
            addDefaultClasses();
        }
        return entries;
    }

	private void addDefaultClasses() {
		// Add some default entries
		entries.add("java.util.Calendar");
	}
    
    public void resetDefaultValues() {
        entries().clear();
        addDefaultClasses();
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

	public String[] classes() {
		return (String[])entries().toArray(new String[entries().size()]);
	}

}
