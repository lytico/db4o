package com.db4odoc.strategies.refactoring.extendhierarchy;


class Human extends Mammal{
    private String name;
    private int iq;

    public Human(String name) {
        this.name = name;
        setBodyTemperature(36);
        setIq(120);
    }
    

    public int getIq() {
        return iq;
    }

    public void setIq(int iq) {
        this.iq = iq;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return "Human{" +
                "name='" + name + '\'' +
                "} is a "+super.toString();
    }
}
