package com.db4odoc.strategies.refactoring.extendhierarchy;


class Mammal {
    private int bodyTemperature;

    public int getBodyTemperature() {
        return bodyTemperature;
    }

    public void setBodyTemperature(int bodyTemperature) {
        this.bodyTemperature = bodyTemperature;
    }

    @Override
    public String toString() {
        return "Mammal{" +
                "bodyTemperature=" + bodyTemperature +
                '}';
    }
}
