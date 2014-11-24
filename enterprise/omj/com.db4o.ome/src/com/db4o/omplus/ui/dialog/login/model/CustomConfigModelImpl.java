/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.model;

import java.io.*;
import java.util.*;

import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;

public class CustomConfigModelImpl implements CustomConfigModel {

	private final CustomConfigSink sink;
	private final ConfiguratorExtractor extractor;
	private final ErrorMessageHandler err;

	private Set<CustomConfigListener> listeners = new HashSet<CustomConfigListener>();

	private List<File> jarFiles = new ArrayList<File>();
	private List<String> configClassNames = new ArrayList<String>();
	private List<String> selectedConfigClassNames = new ArrayList<String>();

	public CustomConfigModelImpl(String[] jarPaths, String[] selectedConfigClassNames, CustomConfigSink sink, ConfiguratorExtractor extractor, ErrorMessageHandler err) {
		this.sink = sink;
		this.extractor = extractor;
		this.err = err;
		addJarPaths(jarPaths);
		selectConfigClassNames(selectedConfigClassNames);
	}

	public void addListener(CustomConfigListener listener) {
		listeners.add(listener);
		notifyListener(listener);
	}

	public void addJarPaths(String... jarPaths) {
		Set<File> jarFileSet = new HashSet<File>(jarFiles);
		for (int jarIdx = 0; jarIdx < jarPaths.length; jarIdx++) {
			try {
				File jarFile = new File(jarPaths[jarIdx]).getCanonicalFile();
				if(!extractor.acceptJarFile(jarFile)) {
					err.error("not an existing jar file: " + jarFile.getAbsolutePath());
					return;
				}
				jarFileSet.add(jarFile);
			} 
			catch (IOException exc) {
				err.error("could not interpret path: " + jarPaths[jarIdx], exc);
				return;
			}
		}
		List<File> jarFileList = new ArrayList<File>(jarFileSet);
		Collections.sort(jarFileList);
		extractConfigNames(jarFileList);
		this.jarFiles = jarFileList;
		notifyListeners();
	}

	public void removeJarPaths(String... jarPaths) {
		List<File> jarFiles = new ArrayList<File>(this.jarFiles);
		for (String jarPath : jarPaths) {
			try {
				jarFiles.remove(new File(jarPath).getCanonicalFile());
			} 
			catch (IOException exc) {
				err.error("could not interpret path: " + jarPath, exc);
			}
		}
		extractConfigNames(jarFiles);
		this.jarFiles = jarFiles;
		notifyListeners();
	}

	public void selectConfigClassNames(String... selectedClassNames) {
		final ArrayList<String> selectedConfigClassNames = new ArrayList<String>(selectedClassNames.length);
		for (String configClassName : selectedClassNames) {
			if(!this.configClassNames.contains(configClassName)) {
				err.error("unknown config class: " + configClassName);
				return;
			}
			selectedConfigClassNames.add(configClassName);
		}
		Collections.sort(selectedConfigClassNames);
		this.selectedConfigClassNames = selectedConfigClassNames;
		notifyListeners();
	}
	
	public void commit() {
		String[] jarPaths = new String[jarFiles.size()];
		for (int jarIdx = 0; jarIdx < jarFiles.size(); jarIdx++) {
			jarPaths[jarIdx] = jarFiles.get(jarIdx).getAbsolutePath();
		}
		sink.customConfig(jarPaths, selectedConfigClassNames.toArray(new String[selectedConfigClassNames.size()]));
	}

	private void extractConfigNames(List<File> jarFileList) {
		Set<String> configNames = new HashSet<String>();
		for (File curJar : jarFileList) {
			try {
				configNames.addAll(extractor.configuratorClassNames(curJar));
			} 
			catch (DBConnectException exc) {
				err.error("could not extract configurators from jar " + curJar.getAbsolutePath() + " - ignoring", exc);
			}
		}
		List<String> originalConfigNames = configClassNames;
		configClassNames = new ArrayList<String>(configNames);
		Collections.sort(configClassNames);
		configNames.removeAll(originalConfigNames);
		selectedConfigClassNames.retainAll(configClassNames);
		selectedConfigClassNames.addAll(configNames);
		Collections.sort(selectedConfigClassNames);
	}

	private void notifyListener(CustomConfigListener listener) {
		notifyListeners(Collections.singleton(listener));
	}	
	
	private void notifyListeners() {
		notifyListeners(listeners);
	}

	private void notifyListeners(Set<CustomConfigListener> listeners) {
		String[] jarPaths = new String[jarFiles.size()];
		for (int jarIdx = 0; jarIdx < jarFiles.size(); jarIdx++) {
			jarPaths[jarIdx] = jarFiles.get(jarIdx).getAbsolutePath();
		}
		for (CustomConfigListener listener : listeners) {
			listener.customConfig(
					jarPaths, 
					configClassNames.toArray(new String[configClassNames.size()]), 
					selectedConfigClassNames.toArray(new String[selectedConfigClassNames.size()]));
		}
	}

}
