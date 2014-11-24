/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.io.*;

import com.db4o.*;
import com.db4o.tools.*;

public class ObjectNotStorable implements Runnable{
	
	private static final String FILE = "notStorable.db4o";
	private static boolean throwException = false;
	
	private String name;
	
	private ObjectNotStorable(String name){
		if(throwException){
			throw new RuntimeException();
		}
		this.name = name;
	}

	public static void main(String[] args) {
		throwException = false;
		new ObjectNotStorable(null).run();
	}
	
	public void run(){
		new File(FILE).delete();
		Db4o.configure().exceptionsOnNotStorable(true);
		run1();
	}
	
	private void run1(){
		try{
			setExc();
		}catch(Exception e){
			e.printStackTrace();
		}
		try{
			getExc();
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
	private static void setOK(){
		throwException = false;
		ObjectContainer con = Db4o.openFile(FILE);
		ObjectNotStorable ons = new ObjectNotStorable("setOK");
		con.store(ons);
		con.close();
	}
	
	private static void setExc(){
		ObjectContainer con = Db4o.openFile(FILE);
		throwException = false;
		ObjectNotStorable ons = new ObjectNotStorable("setExc");
		throwException = true;
		con.store(ons);
		con.close();
	}
	
	private static void getOK(){
		throwException = false;
		ObjectContainer con = Db4o.openFile(FILE);
		ObjectSet set = con.queryByExample(new ObjectNotStorable(null));
		while(set.hasNext()){
			Logger.log(con, set.next());
		}
		con.close();
	}
	
	private static void getExc(){
		throwException = true;
		ObjectContainer con = Db4o.openFile(FILE);
		ObjectSet set = con.queryByExample(null);
		while(set.hasNext()){
			Logger.log(con, set.next());
		}
		con.close();
		
	}
}
