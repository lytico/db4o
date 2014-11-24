package com.db4odoc.ta.collections;

import com.db4o.activation.ActivationPurpose;
import com.db4o.collections.ActivatableArrayList;

import java.util.Collection;
import java.util.List;

// #example: Using the activation aware collections
public class Team  extends AbstractActivatable{ 

    private List<Pilot> pilots = new ActivatableArrayList<Pilot>();

    public boolean add(Pilot pilot) {
        activate(ActivationPurpose.WRITE);
        return pilots.add(pilot);
    }

    public Collection<Pilot> getPilots(){
        activate(ActivationPurpose.READ);
        return pilots;
    }
}
// #end example
