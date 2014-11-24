/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.ids;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class StandardIdSystemFactory {
	
	public static final byte LEGACY = 0;
	
	public static final byte POINTER_BASED = 1;
	
	public static final byte STACKED_BTREE = 2;
	
	public static final byte DEFAULT = STACKED_BTREE;
	
	public static final byte IN_MEMORY = 3;
	
	public static final byte CUSTOM = 4;
	
	public static final byte SINGLE_BTREE = 5;

	public static IdSystem newInstance(LocalObjectContainer localContainer) {
		SystemData systemData = localContainer.systemData();
		byte idSystemType = systemData.idSystemType();
		
        switch(idSystemType){
	    	case LEGACY:
	    		return new PointerBasedIdSystem(localContainer);
	    	case POINTER_BASED:
	    		return new PointerBasedIdSystem(localContainer);
			case STACKED_BTREE:
				InMemoryIdSystem inMemoryIdSystem = new InMemoryIdSystem(localContainer);
				BTreeIdSystem bTreeIdSystem = new BTreeIdSystem(localContainer, inMemoryIdSystem);
				systemData.freespaceIdSystem(bTreeIdSystem.freespaceIdSystem());
				return new BTreeIdSystem(localContainer, bTreeIdSystem);
			case SINGLE_BTREE:
				InMemoryIdSystem smallInMemoryIdSystem = new InMemoryIdSystem(localContainer);
				BTreeIdSystem smallBTreeIdSystem = new BTreeIdSystem(localContainer, smallInMemoryIdSystem);
				systemData.freespaceIdSystem(smallBTreeIdSystem.freespaceIdSystem());
				return smallBTreeIdSystem;
	    	case IN_MEMORY:
	    		return new InMemoryIdSystem(localContainer);
	    	case CUSTOM:
	    		IdSystemFactory customIdSystemFactory = localContainer.configImpl().customIdSystemFactory();
	    		if(customIdSystemFactory == null){
	    			throw new Db4oFatalException("Custom IdSystem configured but no factory was found. See IdSystemConfiguration#useCustomSystem()");
	    		}
	    		return customIdSystemFactory.newInstance(localContainer);
	        default:
	        	return new PointerBasedIdSystem(localContainer);
        }
	            
    }
	

}
