/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs;

import java.util.Iterator;
import java.util.LinkedList;

import org.eclipse.jface.preference.PreferenceDialog;
import org.eclipse.jface.preference.PreferenceManager;
import org.eclipse.jface.preference.PreferenceNode;
import org.eclipse.jface.resource.ImageDescriptor;
import org.eclipse.swt.widgets.Shell;

public class PreferenceUI {

	private static PreferenceUI preferenceUI = null;
	
	/**
	 * Get the PreferenceUI object
	 * 
	 * @return The PreferenceUI singleton
	 */
	public static PreferenceUI getDefault() {
		if (preferenceUI == null) {
			preferenceUI = new PreferenceUI();
		}
		return preferenceUI;
	}
	
	private static LinkedList preferencePages = new LinkedList();
	
	private static class PreferencePageNode {
		public final String id;
		public final String name;
		public final ImageDescriptor image;
		public final String preferencePageClassName;

		public PreferencePageNode(String id, String name, ImageDescriptor image, String preferencePageClassName) {
			this.id = id;
			this.name = name;
			this.image = image;
			this.preferencePageClassName = preferencePageClassName;
		}
	}
	
	/**
	 * Register a preference page with the preferences UI.  In our implementation,
	 * all preference pages are in a flat list, not in a hierarchy like Eclipse's.
	 * <p>
	 * Note: adding preference page nodes is not intended to be thread-safe.
	 * If another thread tries to show the dialog at the same time as a
	 * PreferencePageNode is being added, the results are undefined.
	 * 
	 * @param id The page ID string
	 * @param name The page name
	 * @param image An image identifying the page or null if none
	 * @param preferencePage The class of the IPreferencePage implementation
	 */
	public static void registerPreferencePage(String id, String name, ImageDescriptor image, String preferencePageClass) {
		preferencePages.add(new PreferencePageNode(id, name, image, preferencePageClass));
	}
	
	/** Force usage of the singleton */
	private PreferenceUI() {}

	/**
	 * Show the preferences dialog
	 * 
	 * @param shell The parent shell to use.
	 */
	public void showPreferencesDialog(Shell shell) {
		PreferenceManager manager = new PreferenceManager();
		addPreferencePages(manager);
		PreferenceDialog dialog = new PreferenceDialog(shell, manager);
		dialog.setBlockOnOpen(true);
		dialog.open();
	}

	private void addPreferencePages(PreferenceManager manager) {
		for (Iterator i = preferencePages.iterator(); i.hasNext();) {
			PreferencePageNode node = (PreferencePageNode) i.next();
			manager.addToRoot(new PreferenceNode(node.id, node.name, node.image, node.preferencePageClassName));
		}
	}

}
