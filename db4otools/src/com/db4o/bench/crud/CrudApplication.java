/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.bench.crud;

import java.io.*;

import com.db4o.*;
import com.db4o.bench.logging.*;
import com.db4o.config.*;
import com.db4o.io.*;

/**
 * Very simple CRUD (Create, Read, Update, Delete) application to 
 * produce log files as an input for I/O-benchmarking.
 */
public class CrudApplication {
	
	
	private static final String DATABASE_FILE = "simplecrud.db4o";
	
	
	public void run(int itemCount) {
		deleteDbFile();
		create(itemCount, newConfiguration(itemCount));
		read(newConfiguration(itemCount));
		update(newConfiguration(itemCount));
		delete(newConfiguration(itemCount));
		deleteDbFile();
	}

	private void create(int itemCount, EmbeddedConfiguration config) {
		ObjectContainer oc = open(config);
		for (int i = 0; i < itemCount; i++) {
			oc.store(Item.newItem(i));
			// preventing heap space problems by committing from time to time
			if(i % 100000 == 0) {
				oc.commit();
			}
		}
		oc.commit();
		oc.close();
	}
	
	private void read(EmbeddedConfiguration config) {
		ObjectContainer oc = open(config);
		ObjectSet objectSet = allItems(oc);
		while(objectSet.hasNext()){
			Item item = (Item) objectSet.next();
		}
		oc.close();
	}
	
	private void update(EmbeddedConfiguration config) {
		ObjectContainer oc = open(config);
		ObjectSet objectSet = allItems(oc);
		while(objectSet.hasNext()){
			Item item = (Item) objectSet.next();
			item.change();
			oc.store(item);
		}
		oc.close();
	}

	private void delete(EmbeddedConfiguration config) {
		ObjectContainer oc = open(config);
		ObjectSet objectSet = allItems(oc);
		while(objectSet.hasNext()){
			oc.delete(objectSet.next());
			// adding commit results in more syncs in the log, 
			// which is necessary for meaningful statistics!
			oc.commit();	 
		}
		oc.close();
	}

	private EmbeddedConfiguration newConfiguration(int itemCount) {
		FileStorage rafAdapter = new FileStorage();
		Storage ioAdapter = new LoggingStorage(rafAdapter, logFileName(itemCount));
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(ioAdapter);
		return config;
	}

	private void deleteDbFile() {
		new File(DATABASE_FILE).delete();
	}

	private ObjectSet allItems(ObjectContainer oc) {
		return oc.query(Item.class);
	}

	private ObjectContainer open(EmbeddedConfiguration config) {
		return Db4oEmbedded.openFile(config, DATABASE_FILE);
	}

	public static String logFileName(int itemCount) {
		return "simplecrud_" + itemCount + ".log";
	}
	
}
