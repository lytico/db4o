package com.db4odoc.disconnectedobj.idexamples;

public class Pilot extends IDHolder {
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
