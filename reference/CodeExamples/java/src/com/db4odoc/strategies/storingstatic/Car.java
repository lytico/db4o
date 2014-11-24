package com.db4odoc.strategies.storingstatic;


public class Car {
    private Color color = Color.BLACK;

    public Car() {
    }

    public Car(Color color) {
        this.color = color;
    }

    public Color getColor() {
        return color;
    }

    public void setColor(Color color) {
        this.color = color;
    }
}
