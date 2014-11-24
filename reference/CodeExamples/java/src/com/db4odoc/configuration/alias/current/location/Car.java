package com.db4odoc.configuration.alias.current.location;


public class Car {
    private Pilot pilot = new Pilot();

    public Pilot getPilot() {
        return pilot;
    }

    public void setPilot(Pilot pilot) {
        this.pilot = pilot;
    }
}