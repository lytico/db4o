/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.metadata;

import com.db4o.internal.*;

/**
 * 
 * ClassMetadata instances are stored by VodReplicationProvider
 * in #ensureClassKnown.
 * EventProcessor listens to creation of ClassMetadata instances.
 * It modifies the monitored boolean as soon as the listener channel
 * for the class is up. This way we can wait in the 
 * ReplicationProvider until the monitored boolean is set, so we 
 * don't loose any events for the very first stored objects.
 */
public class ClassMetadata extends VodLoidAwareObject {
	
	private String name;
	
	private String fullyQualifiedName;
	
	public ClassMetadata() {
	}
	
	public ClassMetadata(String name, String fullyQualifiedName, boolean monitored){
		this.name = name;
		this.fullyQualifiedName = fullyQualifiedName;
	}


	public ClassMetadata(String name, String fullyQualifiedName){
		this(name, fullyQualifiedName, false);
	}
	
	@Override
	public String toString() {
		return Reflection4.dump(this);
	}
	
	@Override
	public boolean equals(Object obj) {
		if(obj == this){
			return true;
		}
		if(! (obj instanceof ClassMetadata)){
			return false;
		}
		return fullyQualifiedName.equals(((ClassMetadata) obj).fullyQualifiedName);
	}
	
	@Override
	public int hashCode() {
		return fullyQualifiedName.hashCode();
	}

	public String name() {
		return name;
	}

	public String fullyQualifiedName() {
		return fullyQualifiedName;
	}
}
