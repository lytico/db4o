/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.eventprocessor;

import java.util.*;

import javax.jdo.*;

import com.db4o.drs.versant.*;

public class CreateEventSchema {
	
	public static void main(String[] args) {
		
		if(args.length != 1){
			System.out.println("Usage: CreateEventSchema [databaseName]");
			return;
		}
		String databaseName = args[0];
		VodDatabase vod = new VodDatabase(databaseName, new Properties());
		
		if(! vod.dbExists()){
			System.out.println("Database '" + databaseName + "' does not exist.");
		}
		
		System.out.println("Creating event schema for database '" + databaseName + "'");
		
		VodJvi jvi =  new VodJvi(vod);
		jvi.createEventSchema();
		PersistenceManager pm = vod.persistenceManagerFactory().getPersistenceManager();
		pm.close();
	}

}
