package com.db4odoc.tutorial.utils;

/**
 * @author roman.stoffel@gamlor.info
 * @since 25.07.2010
 */
public final class Modifiable<T> {
    private T value;

    public static Modifiable<Boolean> create(boolean bool) {
        return new Modifiable<Boolean>(bool);
    }

    public static <T> Modifiable<T> create(T value) {
        return new Modifiable<T>(value);
    }

    public Modifiable(T value) {
        this.value = value;
    }

    public T getValue() {
        return value;
    }

    public void setValue(T value) {
        this.value = value;
    }
}

