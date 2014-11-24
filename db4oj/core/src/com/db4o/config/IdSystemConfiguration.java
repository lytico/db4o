/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.config;

/**
 * Interface to configure the IdSystem.
 */
public interface IdSystemConfiguration {
	
	/**
	 * configures db4o to store IDs as pointers.
	 */
	public void usePointerBasedSystem();
	
	/**
	 * configures db4o to use a stack of two BTreeIdSystems on 
	 * top of an InMemoryIdSystem. This setup is scalable for
	 * large numbers of IDs. It is the default configuration
	 * when new databases are created.
	 */
	public void useStackedBTreeSystem();
	
	
	/**
	 * configures db4o to use a single BTreeIdSystem on
	 * top of an InMemoryIdSystem. This setup is suitable for 
	 * smaller databases with a small number of IDs.
	 * For larger numbers of IDs call {@link #useStackedBTreeSystem()}.
	 */
	public void useSingleBTreeSystem();
	
	
	/**
	 * configures db4o to use an in-memory ID system.
	 * All IDs get written to the database file on every commit.
	 */
	public void useInMemorySystem();
	
	/**
	 * configures db4o to use a custom ID system.
	 * Pass an {@link IdSystemFactory} that creates the IdSystem.
	 * Note that this factory has to be configured every time you
	 * open a database that you configured to use a custom IdSystem.
	 */
	public void useCustomSystem(IdSystemFactory factory);

}
