package com.db4odoc.disconnectedobj.objectidentity;

public class Pilot {
	private String name;

    public Pilot(String name)  {
        this.name=name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getName()  {
        return name;
    }

    public String toString()  {
        return name;
    }
}
