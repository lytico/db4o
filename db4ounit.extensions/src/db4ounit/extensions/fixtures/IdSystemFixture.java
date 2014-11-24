/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.internal.config.*;
import com.db4o.internal.ids.*;

public class IdSystemFixture extends Db4oSolo {
	
	private final byte _idSystemType;
	
	 public IdSystemFixture(byte idSystemType) {
		_idSystemType = idSystemType;
	 }
	
	public IdSystemFixture() {		
		_idSystemType = StandardIdSystemFactory.STACKED_BTREE;
	}
	
    protected ObjectContainer createDatabase(Configuration config) {
    	EmbeddedConfiguration embeddedConfiguration = Db4oLegacyConfigurationBridge.asEmbeddedConfiguration(config);
    	
        switch(_idSystemType){
	    	case StandardIdSystemFactory.POINTER_BASED:
	    		embeddedConfiguration.idSystem().usePointerBasedSystem();
	    		break;
	    	case StandardIdSystemFactory.STACKED_BTREE:
	    		embeddedConfiguration.idSystem().useStackedBTreeSystem();
	    		break;
	    	case StandardIdSystemFactory.IN_MEMORY:
	    		embeddedConfiguration.idSystem().useInMemorySystem();
	    		break;
	    	default:
	    		throw new IllegalStateException();
	    		
        }
        
        // embeddedConfiguration.file().freespace().useBTreeSystem();
        
        return super.createDatabase(config);
    }

    public String label() {
    	String idSystemType = "";
        switch(_idSystemType){
	    	case StandardIdSystemFactory.POINTER_BASED:
	    		idSystemType = "PointerBased";
	    		break;
	    	case StandardIdSystemFactory.STACKED_BTREE:
	    		idSystemType = "BTree";
	    		break;
	    	case StandardIdSystemFactory.IN_MEMORY:
	    		idSystemType = "InMemory";
	    		break;
	    	default:
	    		throw new IllegalStateException();
        }
        return "IdSystem-" + idSystemType + " " + super.label();
    }
    
    public boolean accept(Class clazz) {
        return super.accept(clazz)
                && !OptOutIdSystem.class.isAssignableFrom(clazz);
    }


}
