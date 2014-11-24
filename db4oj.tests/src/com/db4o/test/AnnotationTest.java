/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.io.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AnnotationTest {
	
	

	public static void main(String[] args) {
		
		String uri = "file.db4o";
		Storage storage = new MemoryStorage();
		
		ObjectContainer db = openObjectContainer(storage, uri);
		try {
			fillDBwithSheeps(db);
			db = openObjectContainer(storage, uri);
			retriveSheep(db);

			db = openObjectContainer(storage, uri);
			System.out.println("\n");
			updateSheep(db);

			db = openObjectContainer(storage, uri);
			System.out.println("\n");
			deleteSheep(db);

		} finally {
			db.close();
		}
	}
	
	private static ObjectContainer openObjectContainer(Storage storage, String uri){
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(storage);
		return Db4oEmbedded.openFile(config, uri);
	}

	private static void updateSheep(ObjectContainer db) {
		System.out.println("updating annotated sheeps");
		Sheep sheep = (Sheep) db.queryByExample(new Sheep("test7", null)).next();
		sheep.setName(sheep.getName() + "_new");
		db.store(sheep);
		ObjectSet<Sheep> result = db.queryByExample(Sheep.class);
		while (result.hasNext()) {
			System.out.println(result.next());
		}

		System.out.println("updating not annotated sheeps");
		SheepNotAnnotated notAnnot = (SheepNotAnnotated) db.queryByExample(
				new SheepNotAnnotated("notAnnotTest7", null)).next();
		notAnnot.setName(notAnnot.getName() + "_new");
		db.store(notAnnot);
		ObjectSet<SheepNotAnnotated> res = db.queryByExample(SheepNotAnnotated.class);
		while (res.hasNext()) {
			System.out.println(res.next());

		}

		db.close();
	}

	private static void deleteSheep(ObjectContainer db) {
		System.out.println("deleting annotated sheeps");
		Sheep sheep = (Sheep) db.queryByExample(new Sheep("test15", null)).next();
		db.delete(sheep);
		ObjectSet<Sheep> result = db.queryByExample(Sheep.class);
		while (result.hasNext()) {
			System.out.println(">>" + (Sheep) result.next());
		}
		System.out.println("deleting not annotated sheeps");
		SheepNotAnnotated notAnnot = (SheepNotAnnotated) db.queryByExample(
				new SheepNotAnnotated("notAnnotTest15", null)).next();
		db.delete(notAnnot);
		ObjectSet<SheepNotAnnotated> res = db.queryByExample(SheepNotAnnotated.class);
		while (res.hasNext()) {
			System.out.println(">>>" + (SheepNotAnnotated) res.next());
		}
		db.commit();
		db.close();
	}

	private static void retriveSheep(ObjectContainer db) {
		System.out.println("retriving annotated sheeps");
		ObjectSet<Sheep> result = db.queryByExample(new Sheep("test23", null));
		System.out.println((Sheep) result.next());

		System.out.println("retriving not annotated sheeps");
		ObjectSet<SheepNotAnnotated> res = db.queryByExample(new SheepNotAnnotated(
				"notAnnotTest23", null));
		System.out.println((SheepNotAnnotated) res.next());
		db.close();
	}

	private static void fillDBwithSheeps(ObjectContainer db) {
		Sheep parent = null;
		for (int i = 0; i < 36; i++) {
			Sheep sheep = new Sheep("test" + i, parent);
			db.store(sheep);
			parent = sheep;
		}

		SheepNotAnnotated notAnnotParent = null;
		for (int j = 0; j < 36; j++) {
			SheepNotAnnotated notAnnotChild = new SheepNotAnnotated(
					"notAnnotTest" + j, notAnnotParent);
			db.store(notAnnotChild);
			notAnnotParent = notAnnotChild;
		}

		db.commit();
		db.close();
	}

}
