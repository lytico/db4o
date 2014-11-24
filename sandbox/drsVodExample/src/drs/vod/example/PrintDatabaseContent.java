/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example;

import java.util.*;

import javax.jdo.*;

import com.db4o.*;

import drs.vod.example.model.*;
import drs.vod.example.utils.*;

public class PrintDatabaseContent {
	
	public static void main(String[] args) {
		printVODContent();
		printDb4oContent();
	}

	private static void printVODContent() {
		PersistenceManager pm = VodHelper.getPersistenceManager();
		pm.currentTransaction().begin();
		System.out.println("\nVOD database dRSVodExample");
		printLine();
		printClassInstances(pm, Book.class);
		printClassInstances(pm, Customer.class);
		printClassInstances(pm, Order.class);
		pm.currentTransaction().commit();
		pm.close();
	}

	private static void printClassInstances(PersistenceManager pm, Class clazz) {
		Collection result = (Collection) pm.newQuery(clazz).execute();
		System.out.println("Instances for " + clazz.getName() + ": " + result.size());
		for (Object obj: result) {
			System.out.println(obj);
		}
	}

	private static void printDb4oContent() {
		ObjectContainer objectContainer = Db4oHelper.openObjectContainer();
		System.out.println("\ndb4o database file dRSVodExample.db4o");
		printLine();
		printClassInstances(objectContainer, Book.class);
		printClassInstances(objectContainer, Customer.class);
		printClassInstances(objectContainer, Order.class);
		objectContainer.close();
	}

	private static void printClassInstances(ObjectContainer objectContainer, Class clazz) {
		ObjectSet result = objectContainer.query(clazz);
		System.out.println("Instances for " + clazz.getName() + ": " + result.size());
		for (Object obj: result ) {
			System.out.println(obj);
		}
	}

	private static void printLine() {
		System.out.println("*************************************************************");
	}
	

}
