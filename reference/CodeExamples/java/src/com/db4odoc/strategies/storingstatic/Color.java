package com.db4odoc.strategies.storingstatic;

// #example: Class as enumeration
public final class Color {
    public final static Color BLACK = new Color(0,0,0);
    public final static Color WHITE = new Color(255,255,255);
    public final static Color RED = new Color(255,0,0);
    public final static Color GREEN = new Color(0,255,0);
    public final static Color BLUE = new Color(0,0,255);

    private final int red;
    private final int green;
    private final int blue;

    private Color(int red, int green, int blue) {
        this.red = red;
        this.green = green;
        this.blue = blue;
    }

    public int getRed() {
        return red;
    }

    public int getGreen() {
        return green;
    }

    public int getBlue() {
        return blue;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        Color color = (Color) o;

        if (blue != color.blue) return false;
        if (green != color.green) return false;
        if (red != color.red) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = red;
        result = 31 * result + green;
        result = 31 * result + blue;
        return result;
    }

    @Override
    public String toString() {
        return "Color{" +
                "red=" + red +
                ", green=" + green +
                ", blue=" + blue +
                '}';
    }
}
// #end example
