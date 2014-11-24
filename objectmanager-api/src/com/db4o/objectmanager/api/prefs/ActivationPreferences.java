/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.api.prefs;

import com.db4o.Db4o;

public class ActivationPreferences {

	public static final String ACTIVATION_PREFERENCES_ID = "Activation";

	public static ActivationPreferences getDefault() {
		// If the object already exists in the preference store, just return
		// it. Otherwise, create it.
		ActivationPreferences result = (ActivationPreferences) Preferences
                .getDefault().getPreference(ACTIVATION_PREFERENCES_ID);

		if (result == null) {
			result = new ActivationPreferences();
		}
		return result;
	}

	// Constants for default preference values...
	public static final int DEFAULT_INITIAL_ACTIVATION_DEPTH = 5;

	public static final int DEFAULT_SUBSEQUENT_ACTIVATION_DEPTH = 2;

	private int initialActivationDepth = DEFAULT_INITIAL_ACTIVATION_DEPTH;

	private int subsequentActivationDepth = DEFAULT_SUBSEQUENT_ACTIVATION_DEPTH;

	/**
	 * @return Returns the initialActivationDepth.
	 */
	public int getInitialActivationDepth() {
		return initialActivationDepth;
	}

	/**
	 * @param initialActivationDepth
	 *            The initialActivationDepth to set.
	 */
	public void setInitialActivationDepth(int initialActivationDepth) {
		this.initialActivationDepth = initialActivationDepth;
		Db4o.configure().activationDepth(initialActivationDepth);
	}

	/**
	 * @return Returns the subsequentActivationDepth.
	 */
	public int getSubsequentActivationDepth() {
		return subsequentActivationDepth;
	}

	/**
	 * @param subsequentActivationDepth
	 *            The subsequentActivationDepth to set.
	 */
	public void setSubsequentActivationDepth(int subsequentActivationDepth) {
		this.subsequentActivationDepth = subsequentActivationDepth;
	}

	/**
	 * Reset all values to their defaults.
	 */
	public void resetDefaultValues() {
		setInitialActivationDepth(DEFAULT_INITIAL_ACTIVATION_DEPTH);
		setSubsequentActivationDepth(DEFAULT_SUBSEQUENT_ACTIVATION_DEPTH);
	}
	
	/*
	 * FIXME: Per-class activation settings go here
	 */
	
//	private HashMap classActivations = new HashMap();

}
