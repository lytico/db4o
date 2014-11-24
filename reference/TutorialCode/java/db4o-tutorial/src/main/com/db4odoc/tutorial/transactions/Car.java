package com.db4odoc.tutorial.transactions;


import com.db4odoc.tutorial.transparentpersistence.TransparentPersisted;

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
}
// #end example
