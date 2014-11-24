package com.db4odoc.strategies.refactoring.removehierarchy;


class Primate extends Mammal {
    private int iq;

    public int getIq() {
        return iq;
    }

    public void setIq(int iq) {
        this.iq = iq;
    }

    @Override
    public String toString() {
        return "Primate{" +
                "iq=" + iq +
                "} is a "+super.toString();
    }
}