/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package stepbystep;

import java.io.*;
import java.util.*;

import org.hibernate.*;
import org.hibernate.criterion.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.hibernate.*;
import com.db4o.drs.hibernate.impl.*;
import com.db4o.query.*;

public class StepByStepExample  {
	
	protected String db4oFileName = "StepByStepExample.db4o";
	
	protected String hsqldbFileName =  "stepbystep.hsqldb";
	
	protected final String hibernateConfigurationFileName = "stepbystep/hibernate.cfg.xml";

	public static void main(String[] args) {
		new StepByStepExample().run();
	}

	public void run() {
		deleteDatabaseFiles();

		System.out.println("Running StepByStep Example.");

		storePilotAndCarToDb4o();
		
		printDatabaseContent();

		replicateAllToHibernate();

		printDatabaseContent();
		
		modifyPilotInHibernate();

		printDatabaseContent();
		
		replicateChangesToDb4o();
		
		printDatabaseContent();

		modifyPilotAndCarInDb4o();
		
		printDatabaseContent();
		
		replicatePilotsToHibernate();
		
		printDatabaseContent();

		System.out.println("StepByStep Example completed");

		deleteDatabaseFiles();
	}

	private void printDatabaseContent() {
		System.out.println("\nDatabase Content");
		printLine();
		System.out.println("db4o");
		printLine();
		ObjectContainer objectContainer = openObjectContainer(db4oFileName);
		ObjectSet objectsInDb4o = objectContainer.queryByExample(new Object());
		for(Object obj : objectsInDb4o){
			System.out.println(obj);
		}
		objectContainer.close();
		printLine();
		System.out.println("Hibernate");
		printLine();
		org.hibernate.cfg.Configuration cfg = new org.hibernate.cfg.Configuration().configure(hibernateConfigurationFileName);
		SessionFactory sessionFactory = cfg.buildSessionFactory();
		Session session = sessionFactory.openSession();
		List objectsInHibernate = session.createCriteria(Object.class).list();
		for(Object obj : objectsInHibernate){
			System.out.println(obj);
		}
		session.close();
		printLine();
		System.out.println();
	}

	private void printLine() {
		System.out.println("---------------------------------------------");
	}

	private void storePilotAndCarToDb4o() {
		System.out.println("Creating pilot and car objects");
		Car car = new Car("BMW", "M3", 272);
		System.out.println(car);
		
		Pilot pilot = new Pilot("John", car, 100);
		System.out.println(pilot);

		System.out.println("Opening the db4o database");
		ObjectContainer db4o = openObjectContainer(db4oFileName);
		
		System.out.println("Saving pilot and car to db4o");
		db4o.store(pilot);
		db4o.commit();
		db4o.close();
		System.out.println("Committed and closed db4o");
	}

	private void replicateAllToHibernate() {
		System.out.println("Re-opening db4o");
		ObjectContainer objectContainer = openObjectContainer(db4oFileName);

		org.hibernate.cfg.Configuration config = 
			new org.hibernate.cfg.Configuration().configure(hibernateConfigurationFileName);

		System.out.println("Starting the first round of replication between db4o and Hibernate");
		Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(objectContainer);
		HibernateReplicationProvider providerB = new HibernateReplicationProvider(config);
		ReplicationSession replication = Replication.begin(providerA, providerB);

		ObjectSet modifiedObjects = replication.providerA().objectsChangedSinceLastReplication();
		System.out.println("Iterating all objects changed in db4o, replicating them to Hibernate");
		while (modifiedObjects.hasNext()){
			Object modifiedObject = modifiedObjects.next();
			System.out.println("Replicating " + modifiedObject);
			replication.replicate(modifiedObject);
		}

		System.out.println("Committing replication.");
		replication.commit();
		replication.close();
		objectContainer.close();
		System.out.println("Replication successful.");
	}

	private void modifyPilotInHibernate() {
		org.hibernate.cfg.Configuration cfg = 
			new org.hibernate.cfg.Configuration().configure(hibernateConfigurationFileName);

		System.out.println("Configuring Hibernate to listen to object update events");
		ReplicationConfigurator.configure(cfg);

		SessionFactory sessionFactory = cfg.buildSessionFactory();
		Session session = sessionFactory.openSession();

		System.out.println("Installing the object update event listener");
		ReplicationConfigurator.install(session, cfg);

		Transaction tx = session.beginTransaction();

		System.out.println("Finding Pilot 'John' in Hibernate.");
		Pilot john = (Pilot) session.createCriteria(Pilot.class) .add(Restrictions.eq("name", "John")).list().get(0);

		System.out.println("Changing points of Pilot 'John' to 142");
		john.setPoints(142);

		session.flush();
		tx.commit();

		session.close();
		sessionFactory.close();

		System.out.println("Changes committed to Hibernate.");

	}

	private void replicateChangesToDb4o() {
		System.out.println("Re-opening db4o");
		ObjectContainer db4o = openObjectContainer(db4oFileName);

		org.hibernate.cfg.Configuration config = new org.hibernate.cfg.Configuration().configure(hibernateConfigurationFileName);

		System.out.println("Starting the second round of replication between db4o and Hibernate");
		Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(db4o);
		HibernateReplicationProvider providerB = new HibernateReplicationProvider(config);
		ReplicationSession replication = Replication.begin(providerA, providerB);


		ObjectSet modifiedObjects = replication.providerB().objectsChangedSinceLastReplication();
		System.out.println("Iterating all objects changed in Hibernate, replicating them to db4o");
		while (modifiedObjects.hasNext()){
			Object modifiedObject = modifiedObjects.next();
			System.out.println("Replicating " + modifiedObject);
			replication.replicate(modifiedObject);
		}

		replication.commit();
		System.out.println("Committed replication.");
		replication.close();
		db4o.close();
		System.out.println("Replication is successful.");
	}

	private void modifyPilotAndCarInDb4o() {
		System.out.println("Re-opening the db4o database");
		ObjectContainer db4o = openObjectContainer(db4oFileName);

		System.out.println("Finding Pilot 'John' in Db4o.");

		Pilot pilot = db4o.query(new Predicate<Pilot>() {
			public boolean match(Pilot p) {
				return p.name.equals("John");
			}
		}).next();

		System.out.println("Changing Pilot 'John's points to 169");
		pilot.setPoints(169);
		db4o.store(pilot);
		System.out.println("Changing the Car horsepower to 390 ");
		pilot.car.setHorsePower(390);
		db4o.store(pilot.car);
		
		db4o.commit();
		db4o.close();
		System.out.println("Committed and closed db4o");
	}

	private void replicatePilotsToHibernate() {
		System.out.println("Re-opening db4o");
		ObjectContainer db4o = openObjectContainer(db4oFileName);

		System.out.println("Reading the Hibernate configuration file");
		org.hibernate.cfg.Configuration config = 
			new org.hibernate.cfg.Configuration().configure(hibernateConfigurationFileName);

		System.out.println("Starting the final round of replication between db4o and Hibernate");
		Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(db4o);
		HibernateReplicationProvider providerB = new HibernateReplicationProvider(config);
		ReplicationSession replication = Replication.begin(providerA, providerB);


		ObjectSet allObjects = replication.providerA().objectsChangedSinceLastReplication(Pilot.class);
		System.out.println("Iterating all Pilots changed in db4o, replicating them to Hibernate");
		while (allObjects.hasNext())
			replication.replicate(allObjects.next());

		replication.commit();
		System.out.println("Commited the replication.");
		replication.close();
		db4o.close();

		System.out.println("Replication is successful.");
	}
	
	protected ObjectContainer openObjectContainer(String fileName) {
		return Db4oEmbedded.openFile(embeddedDb4oConfiguration(), fileName);
	}
	
	protected EmbeddedConfiguration embeddedDb4oConfiguration(){
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		// The following two configuration settings are required
		// for db4o replication to work:
		config.file().generateUUIDs(ConfigScope.GLOBALLY);
		config.file().generateCommitTimestamps(true);
		return config;
	}
	
	protected void deleteDatabaseFiles() {
		new File(db4oFileName).delete();
		new File(hsqldbFileName).delete();
		new File(hsqldbFileName + ".log").delete();
		new File(hsqldbFileName + ".lck").delete();
		new File(hsqldbFileName + ".properties").delete();
		new File(hsqldbFileName + ".script").delete();
	}

}
