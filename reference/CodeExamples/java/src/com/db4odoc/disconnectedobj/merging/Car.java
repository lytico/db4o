package com.db4odoc.disconnectedobj.merging;


public class Car extends IDHolder{
    private Pilot pilot;
    private String name;

    public Car(Pilot pilot, String name) {
        this.pilot = pilot;
        this.name = name;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Pilot getPilot() {
        return pilot;
    }

    public void setPilot(Pilot pilot) {
        this.pilot = pilot;
    }

    @Override
    public String toString() {
        return name + " pilot: " + pilot;
    }
}