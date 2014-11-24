/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.io.*;
import java.util.*;

import com.db4o.*;
import com.db4o.test.types.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class MapEntries {
	
	static String FILE = "hm.db4o";
	
	HashMap hm;

	public static void main(String[] args) {
		// createAndDelete();
		
		set();
		check();
		LogAll.run(FILE);
		update();
		check();
		LogAll.run(FILE);
	}
	
	static void createAndDelete(){
		new File(FILE).delete();
		ObjectContainer con = Db4o.openFile(FILE);
		HashMap map = new HashMap();
		map.put("delme", new Integer(99));
		con.store(map);
		con.close();
		con = Db4o.openFile(FILE);
		con.delete(con.queryByExample(new HashMap()).next());
		con.close();
		LogAll.run(FILE);
	}
	
	static void check(){
		ObjectContainer con = Db4o.openFile(FILE);
		System.out.println("Entry elements: " + con.queryByExample(new com.db4o.config.Entry()).size());
		con.close();
	}
	
	static void set(){
		new File(FILE).delete();
		ObjectContainer con = Db4o.openFile(FILE);
		MapEntries me = new MapEntries();
		me.hm = new HashMap();
		me.hm.put("t1", new ObjectSimplePublic());
		me.hm.put("t2", new ObjectSimplePublic());
		con.store(me);
		con.close();
	}
	
	static void update(){
		ObjectContainer con = Db4o.openFile(FILE);
		ObjectSet set = con.queryByExample(new MapEntries());
		while(set.hasNext()){
			MapEntries me = (MapEntries)set.next();
			me.hm.put("t1", new Integer(100));
			con.store(me.hm);
		}
		con.close();
	}
	
	
}
