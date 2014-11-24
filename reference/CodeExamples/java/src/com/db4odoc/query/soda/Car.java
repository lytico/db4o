package com.db4odoc.query.soda;


class Car {
    private Pilot pilot;
    private String name;

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
