package com.db4odoc.tutorial.demos.firststeps;


public class Car {
    private String carName;

    public Car(String carName) {
        this.carName = carName;
    }


    @Override
    public String toString() {
        return "Car{" +
                "carName='" + carName + '\'' +
                '}';
    }
}
