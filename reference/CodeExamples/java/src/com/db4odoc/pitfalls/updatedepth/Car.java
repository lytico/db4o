package com.db4odoc.pitfalls.updatedepth;


class Car {
    private Person driver;
    private String carName;

    Car(Person driver, String carName) {
        this.driver = driver;
        this.carName = carName;
    }

    public Person getDriver() {
        return driver;
    }

    public void setDriver(Person driver) {
        this.driver = driver;
    }

    public String getCarName() {
        return carName;
    }

    public void setCarName(String carName) {
        this.carName = carName;
    }
}
