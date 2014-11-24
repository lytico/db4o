/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.api.prefs;

import java.io.*;
import java.util.*;
import java.util.logging.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

public class Preferences {
    private static Logger logger = Logger.getLogger(Preferences.class.getName());

    private static Preferences prefs = null;

    private static transient ObjectContainer db;

    private static transient final String preferencesFile = new File(new File(System
			.getProperty("user.home")), ".objectmanager2.yap")
			.getAbsolutePath();

    public static final String FRAME_SIZE = "frameSize";
    public static final String FRAME_LOCATION = "frameLocation";

    /**
	 * Called by BrowserCore during initialization
	 */
	public static void initialize() {
		// Register the preference pages in the UI
		//getDefault().registerPreferences();
	}

	/**
	 * Singleton implementation
	 * 
	 * @return the PreferenceCore singleton
	 */
	public static Preferences getDefault() {
		if (prefs == null) {
			loadOrCreatePreferences();
		}
		return prefs;
	}

	/**
	 * Some preference object has been changed; commit changes to the database
	 */
	public void commit() {
		db.set(prefs);
		db.commit();
	}

	/**
	 * A preference object may have been changed; roll back the change to the
	 * persistent state
	 */
	/*public static void rollback() {
		db.ext().refresh(prefs, Integer.MAX_VALUE);
	}*/

	/**
	 * Close the preference store
	 */
	public static void close() {
		if (db != null)
			db.close();
		db = null;
		prefs = null;
	}

	/*
	 * Load or create the persisted preferences object
	 */
	private static void loadOrCreatePreferences() {
		ObjectClass prefsCore = Db4o.configure().objectClass(
				Preferences.class);
		prefsCore.minimumActivationDepth(Integer.MAX_VALUE);
		prefsCore.updateDepth(Integer.MAX_VALUE);

		Db4o.configure().allowVersionUpdates(true);
		db = Db4o.openFile(preferencesFile);
		Db4o.configure().allowVersionUpdates(false);
		Query query = db.query();
		query.constrain(Preferences.class);
		ObjectSet result = query.execute();

		if (result.hasNext()) {
			prefs = (Preferences) result.next();
		} else {
			prefs = new Preferences();
		}

		if (result.size() > 1) {
			rebuildCorruptDatabase(result.size());
		}

		ActivationPreferences activations = (ActivationPreferences) prefs
				.getPreference(ActivationPreferences.ACTIVATION_PREFERENCES_ID);
	}

	/*
	 * If we detected a corrupt preferences database, back it up and rebuild a
	 * new copy.
	 */
	private static void rebuildCorruptDatabase(int resultsize) {
		logger.info(resultsize
				+ " instances of Preferences found in the database.");
		String backupFile = preferencesFile + ".bkp";
        logger.info("Backing up database to " + backupFile);
		try {
			db.ext().backup(backupFile);
		} catch (RuntimeException e) {
            logger.info("Couldn't create backup file.");
			e.printStackTrace();
		}
		db.close();
		new File(preferencesFile).delete();
		db = Db4o.openFile(preferencesFile);
		db.set(prefs);
		db.commit();
	}

	// --------------------------------------------------

	private HashMap preferenceStore = new HashMap();

	/**
	 * Method registerPreferences. Registers a preference page and associated
	 * model object.
	 *
	 * @param key
	 *            The String ID used to reference this preference
	 * @param preference
	 *            The preference Object used to store the preferences
	 */
	/*public void registerPreferences(String key, String name,
			ImageDescriptor image, Class preferencePageClass, Object preference) {
		PreferenceUI.registerPreferencePage(key, name, image,
				preferencePageClass.getName());
		setPreference(key, preference);
	}*/

	public void setPreference(String key, Object preference) {
		long start = System.currentTimeMillis();
		preferenceStore.put(key, preference);
        commit();
		long end = System.currentTimeMillis();
		long duration = end - start;
		//System.out.println("Time to set preferences: " + duration);
	}

	/**
	 * Method getPreference. Return the preference object associated with a
	 * particular preference page ID.
	 * 
	 * @param id
	 *            The preference page ID.
	 * @return The associated preference page object.
	 */
	public Object getPreference(String id) {
		return preferenceStore.get(id);
	}

	// --------------------------------------------------

	/**
	 * Register all preference pages in the application here
	 */
	/*private void registerPreferences() {
		registerPreferences(ActivationPreferences.ACTIVATION_PREFERENCES_ID,
				"Object Activation", null, ActivationPreferencePage.class,
				ActivationPreferences.getDefault());
        registerPreferences(ClasspathPreferences.CLASSPATH_PREFERENCES_ID,
                "Classpath", null, ClasspathPreferencePage.class,
                ClasspathPreferences.getDefault());
        registerPreferences(ConstructorPreferences.CONSTRUCTOR_PREFERENCES_ID,
        		"Constructor Calling", null, ConstructorPreferencePage.class,
        		ConstructorPreferences.getDefault());
	}*/

}
