/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example;

import com.db4o.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.versant.*;
import com.db4o.drs.versant.jdo.reflect.*;

import drs.vod.example.model.*;
import drs.vod.example.utils.*;

public class ReplicateAllBooksOrdersCustomersFromVodToDb4o {
	
	
	public static void main(String[] args) {
		
		VodDatabase vodDatabase = new VodDatabase("dRSVodExample", VodHelper.properties());
		
		VodReplicationProvider vodReplicationProvider = 
			new VodReplicationProvider(vodDatabase);
		
		ObjectContainer objectContainer = Db4oHelper.openObjectContainer();
		Db4oEmbeddedReplicationProvider db4oReplicationProvider = 
			new Db4oEmbeddedReplicationProvider(objectContainer);
		
		ReplicationSession replicationSession = 
			Replication.begin(vodReplicationProvider, db4oReplicationProvider, new JdoReflector(Thread.currentThread().getContextClassLoader()));
		
		ObjectSet<Book> books = vodReplicationProvider.getStoredObjects(Book.class);
		for (Book book: books) {
			System.out.println("Replicating " + book);
			replicationSession.replicate(book);
		}
		
		ObjectSet<Order> orders = vodReplicationProvider.getStoredObjects(Order.class);
		for (Order order : orders) {
			System.out.println("Replicating " + order);
			replicationSession.replicate(order);
		}
		
		ObjectSet<Customer> customers = vodReplicationProvider.getStoredObjects(Customer.class);
		for (Customer customer: customers) {
			System.out.println("Replicating " + customer);
			replicationSession.replicate(customer);
		}
		
		replicationSession.commit();
		replicationSession.close();
		
		objectContainer.close();
	}


}
