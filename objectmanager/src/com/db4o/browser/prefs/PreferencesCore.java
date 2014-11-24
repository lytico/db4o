/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs;

import java.io.*;
import java.util.*;

import org.eclipse.jface.resource.*;
import org.eclipse.ve.sweet.metalogger.*;

import com.db4o.*;
import com.db4o.browser.prefs.activation.*;
import com.db4o.browser.prefs.classpath.*;
import com.db4o.browser.prefs.constructor.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.query.*;

public class PreferencesCore {
	private static PreferencesCore prefs = null;

	private static transient ObjectContainer db;

	private static transient final String preferencesFile = new File(new File(System
			.getProperty("user.home")), ".objectmanager.yap")
			.getAbsolutePath();

	/**
	 * Called by BrowserCore during initialization
	 */
	public static void initialize() {
		// Register the preference pages in the UI
		getDefault().registerPreferences();
	}

	/**
	 * Singleton implementation
	 * 
	 * @return the PreferenceCore singleton
	 */
	public static PreferencesCore getDefault() {
		if (prefs == null) {
			loadOrCreatePreferences();
		}
		return prefs;
	}

	/**
	 * Some preference object has been changed; commit changes to the database
	 */
	public static void commit() {
		db.set(prefs);
		db.commit();
	}

	/**
	 * A preference object may have been changed; roll back the change to the
	 * persistent state
	 */
	public static void rollback() {
		db.ext().refresh(prefs, Integer.MAX_VALUE);
	}

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
				PreferencesCore.class);
		prefsCore.minimumActivationDepth(Integer.MAX_VALUE);
		prefsCore.updateDepth(Integer.MAX_VALUE);

		Db4o.configure().allowVersionUpdates(true);
		db = Db4o.openFile(preferencesFile);
		Db4o.configure().allowVersionUpdates(false);
		Query query = db.query();
		query.constrain(PreferencesCore.class);
		ObjectSet result = query.execute();

		if (result.hasNext()) {
			prefs = (PreferencesCore) result.next();
		} else {
			prefs = new PreferencesCore();
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
		Logger.log().message(resultsize
				+ " instances of PreferencesCore found in the database.");
		String backupFile = preferencesFile + ".bkp";
        Logger.log().message("Backing up database to " + backupFile);
		try {
			db.ext().backup(backupFile);
		} catch (Db4oException e) {
            Logger.log().message("Couldn't create backup file.");
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
	 * @param id
	 *            The String ID used to reference this preference
	 * @param name
	 *            The preference page's descriptive name
	 * @param image
	 *            An image to refer to this preference page or null if none
	 * @param preferencePageClass
	 *            The class of the IPreferencePage
	 * @param preference
	 *            The preference Object used to store the preferences
	 */
	public void registerPreferences(String id, String name,
			ImageDescriptor image, Class preferencePageClass, Object preference) {
		PreferenceUI.registerPreferencePage(id, name, image,
				preferencePageClass.getName());
		registerPreference(id, preference);
	}

	public void registerPreference(String id, Object preference) {
		preferenceStore.put(id, preference);
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
	private void registerPreferences() {
		registerPreferences(ActivationPreferences.ACTIVATION_PREFERENCES_ID,
				"Object Activation", null, ActivationPreferencePage.class,
				ActivationPreferences.getDefault());
        registerPreferences(ClasspathPreferences.CLASSPATH_PREFERENCES_ID,
                "Classpath", null, ClasspathPreferencePage.class,
                ClasspathPreferences.getDefault());
        registerPreferences(ConstructorPreferences.CONSTRUCTOR_PREFERENCES_ID,
        		"Constructor Calling", null, ConstructorPreferencePage.class,
        		ConstructorPreferences.getDefault());
	}

}
