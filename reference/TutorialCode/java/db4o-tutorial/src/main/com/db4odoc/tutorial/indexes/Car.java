package com.db4odoc.tutorial.indexes;

import com.db4o.config.annotations.Indexed;

// #example: Indexing the car name
public class Car {
    @Indexed
    private String carName;
    private int horsePower;

    public Car(String carName, int horsePower) {
        this.carName = carName;
        this.horsePower = horsePower;
    }

    public String getCarName() {
        return carName;
    }

    public int getHorsePower() {
        return horsePower;
    }

}
// #end example

