/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.model;


public interface CustomConfigModel {

	public static interface CustomConfigListener {
		void customConfig(String[] jarPaths, String[] configClassNames, String[] selectedConfigNames);
	}

	void addListener(CustomConfigListener listener);

	void addJarPaths(String... jarPaths);

	void removeJarPaths(String... jarPaths);

	void selectConfigClassNames(String... selectedClassNames);

	void commit();

}