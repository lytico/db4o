package com.db4odoc.query.qbe;

import java.util.ArrayList;
import java.util.List;


class Car {
    private Pilot pilot;
    private String name;
    public List<String> tags = new ArrayList<String>();

    Car() {
    }

    public Car(Pilot pilot, String name) {
        this.pilot = pilot;
        this.name = name;
    }

    public Pilot getPilot() {
        return pilot;
    }

    public void setPilot(Pilot pilot) {
        this.pilot = pilot;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return "Car{" +
                "pilot=" + pilot +
                ", name='" + name + '\'' +
                '}';
    }
}