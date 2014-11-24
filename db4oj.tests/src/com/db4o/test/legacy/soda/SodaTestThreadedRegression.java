/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.query.*;
import com.db4o.test.legacy.soda.classes.simple.*;
import com.db4o.test.legacy.soda.collections.*;
import com.db4o.test.legacy.soda.engines.db4o.*;
import com.db4o.test.legacy.soda.wrapper.untyped.*;

@decaf.Ignore(decaf.Platform.JDK11)
public class SodaTestThreadedRegression extends SodaTest implements Runnable{
	
	private static final Object lock = new Object();
	private static int RUNS = 300;
	
	private final STClass[] classes;
	private static volatile int runningThreads;
	
	
	public SodaTestThreadedRegression(STClass[] classes){
		this.classes = classes;
		setSodaTestOn(classes);
	}
	
	public static void main(String[] args) {
		
		cascadeOnDelete(new STArrayListT());
		cascadeOnDelete(new STOwnCollectionW());
		
		begin();
		
		time = System.currentTimeMillis();
		
		engine = new STDb4o();
		// engine = new STDb4oClientServer();
		
		engine.reset();
		engine.open();
		
		startThread(new STClass[] {new STString()});
		startThread(new STClass[] {new STInteger()});
		startThread(new STClass[] {new STByte()});
		startThread(new STClass[] {new STShort()});
		startThread(new STClass[] {new STBooleanWU()});
		startThread(new STClass[] {new STOwnCollectionW()});
		startThread(new STClass[] {new STArrayListT()});
		
		// We don't want to run out of main to allow sequential
		// execution of Ant tasks.
		do{
			Runtime4.sleep(300);
		}while(runningThreads > 0);
	}
	
	private static void startThread(STClass[] classes){
		for (int i = 0; i < classes.length; i++) {
			if(! jdkOK(classes[i])){
				System.out.println("Test case can't run on this JDK: " + classes[i].getClass().getName());
				return;
			}
		}
		new Thread(new SodaTestThreadedRegression(classes), "SodaTestThreadedRegression.startThread").start();
	}
	
	protected String name(){
		return "S.O.D.A. threaded test";
	}
	
	public void run(){
		String name;
		synchronized(lock){
			runningThreads ++;
			name = "R " + runningThreads + " ";
		}
		Thread.currentThread().setName(name);
		
		for (int i = 0; i < RUNS; i++) {
			if(! quiet){
				System.out.println(name + i);
			}
			store(classes);
			engine.commit();
			test(classes);
			for (int j = 0; j < classes.length; j++) {
				Query q = engine.query();
				q.constrain(classes[j].getClass());
				ObjectSet os = q.execute();
				while(os.hasNext()){
					engine.delete(os.next());
				}
			}
		}
		
		synchronized(lock){
			runningThreads --;
			if(runningThreads < 1){
				engine.close();
				completed();
			}
		}
	}
	
	public static void cascadeOnDelete(Object obj){
		Db4o.configure().objectClass(obj.getClass().getName()).cascadeOnDelete(true);
	}
	
}

