package com.db4odoc.drs.vod;

import javax.jdo.annotations.PersistenceCapable;

@PersistenceCapable
public class Pilot {
	private String name;
	private int points;

    public Pilot() {
    }

    public Pilot(String name)  {
        this.name=name;
    }
    public Pilot(String name, int points)  {
        this.name=name;
        this.points = points;
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
