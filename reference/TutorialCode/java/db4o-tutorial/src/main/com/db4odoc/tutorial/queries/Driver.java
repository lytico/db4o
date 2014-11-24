package com.db4odoc.tutorial.queries;

// #example: Domain model for drivers
public class Driver {
    private String name;
    private int age;
    private Car mostLovedCar;

    public Driver(String name,int age) {
        this.name = name;
        this.age = age;
    }

    public Driver(String name,int age, Car mostLovedCar) {
        this.name = name;
        this.age = age;
        this.mostLovedCar = mostLovedCar;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getName() {
        return name;
    }

    public int getAge() {
        return age;
    }

    public Car getMostLovedCar() {
        return mostLovedCar;
    }

    @Override
    public String toString() {
        return "Driver{" +
                "name='" + name + '\'' +
                ", age=" + age +
                ", mostLovedCar=" + mostLovedCar +
                '}';
    }
}
// #end example