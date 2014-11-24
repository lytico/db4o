package com.db4odoc.drs.rdms;


public class Car {
    private Pilot pilot;
    private String name;
    

    public Car() {
    }

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
