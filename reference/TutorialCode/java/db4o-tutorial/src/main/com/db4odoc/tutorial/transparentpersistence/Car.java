package com.db4odoc.tutorial.transparentpersistence;


// #example: Domain model for cars
@TransparentPersisted
public class Car {
    private String carName;

    public Car(String carName) {
        this.carName = carName;
    }

    public String getCarName() {
        return carName;
    }

    public void setCarName(String carName) {
        this.carName = carName;
    }
}
// #end example
