package com.db4odoc.ta.collections;

import com.db4o.activation.ActivationPurpose;

public class Pilot extends AbstractActivatable{
	private String name;

    public Pilot(String name)  {
        this.name=name;
    }

    public void setName(String name) {
        activate(ActivationPurpose.WRITE);
        this.name = name;
    }

    public String getName()  {
        activate(ActivationPurpose.READ);
        return name;
    }

    public String toString()  {
        return getName();
    }
}