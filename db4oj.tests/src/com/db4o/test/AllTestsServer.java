/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;

/**
 * This class will start a dedicated server for AllTests to run
 * tests on different machines. The server will run the configuration
 * entries also as needed.
 */
public class AllTestsServer extends AllTests implements Runnable{
	
	public static void main(String[] args){
		new AllTestsServer().run();
	}
	
	public void run(){
		Db4o.configure().messageLevel(-1);
		logConfiguration();
		System.out.println("Waiting for tests to be run from different machine.");
		System.out.println("\n\nThe server will need to be closed with CTRL + C.\n\n");
		if(DELETE_FILE){
			Test.delete();
		}
		configure();
		Test.runServer = true;
		Test.clientServer = true;
		Test.open();
	}
}
