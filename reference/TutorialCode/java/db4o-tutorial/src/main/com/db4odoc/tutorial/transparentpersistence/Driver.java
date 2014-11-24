package com.db4odoc.tutorial.transparentpersistence;

import java.util.ArrayList;
import java.util.List;

// #example: Domain model for drivers
@TransparentPersisted
public class Driver {
    private String name;
    private final List<Car> ownedCars = new ArrayList<Car>();
    private Car mostLovedCar;

    public Driver(String name) {
        this.name = name;
    }

    public Driver(String name, Car mostLovedCar) {
        this.name = name;
        this.mostLovedCar = mostLovedCar;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getName() {
        return name;
    }

    public Car getMostLovedCar() {
        return mostLovedCar;
    }
    public Iterable<Car> getOwnedCars() {
        return ownedCars;
    }

    public boolean addOwnedCar(Car car) {
        return ownedCars.add(car);
    }
}
// #end example