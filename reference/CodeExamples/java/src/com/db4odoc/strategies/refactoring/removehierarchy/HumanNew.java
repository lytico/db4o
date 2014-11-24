package com.db4odoc.strategies.refactoring.removehierarchy;


class HumanNew extends Mammal{
    private String name;
    private int iq;


    public HumanNew(String name) {
        this.name = name;
        setBodyTemperature(36);
        setIq(120);
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
    
    public int getIq() {
        return iq;
    }

    public void setIq(int iq) {
        this.iq = iq;
    }

    @Override
    public String toString() {
        return "Human{" +
                "name='" + name + "', " +
                "iq='" + iq + '\'' +
                "} is a "+super.toString();
    }
}