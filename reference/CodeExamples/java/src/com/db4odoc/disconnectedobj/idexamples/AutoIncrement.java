package com.db4odoc.disconnectedobj.idexamples;

import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;

import java.util.HashMap;
import java.util.Map;


public class AutoIncrement {
    private PersistedAutoIncrements state = null;
    private final ObjectContainer container;

    public AutoIncrement(ObjectContainer container) {
        this.container = container;
    }


    // #example: getting the next id and storing state
    public synchronized int getNextID(Class forClass) {
        PersistedAutoIncrements incrementState = ensureLoadedIncrements();
        return incrementState.nextNumber(forClass);
	}

    public synchronized void storeState(){
        if(null!=state){
            container.ext().store(state,2);            
        }
    }
    // #end example

    // #example: load the state from the database
    private PersistedAutoIncrements ensureLoadedIncrements() {
        if(null==state){
            state = loadOrCreateState();
        }
        return state;

    }

    private PersistedAutoIncrements loadOrCreateState() {
        ObjectSet<PersistedAutoIncrements> existingState = container.query(PersistedAutoIncrements.class);
        if(0==existingState.size()){
            return new PersistedAutoIncrements(); 
        } else if(1==existingState.size()){
            return existingState.get(0);
        } else{
            throw new IllegalStateException("Cannot have more than one state stored in database");
        }
    }
    // #end example

    // #example: persistent auto increment
    private static class PersistedAutoIncrements {
	    private final Map<Class, Integer> currentHighestIds = new HashMap<Class, Integer>();

        public int nextNumber(Class forClass) {
            Integer number = currentHighestIds.get(forClass);
            if (null == number) {
                number = 0;
            }
            number += 1;
            currentHighestIds.put(forClass,number);
            return number;
        }
    }
    // #end example
}
