package com.db4odoc.callbacks.examples;


class Car {
    private Pilot pilot;

    Car(Pilot pilot) {
        this.pilot = pilot;
    }

    public Pilot getPilot() {
        return pilot;
    }
}
