package com.db4odoc.drs.advanced;

public class Pilot {
	private String name;
	int points;

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

    public int getPoints() {
        return points;
    }

    public void setPoints(int points) {
        this.points = points;
    }

    public String toString()  {
        return name;
    }
}