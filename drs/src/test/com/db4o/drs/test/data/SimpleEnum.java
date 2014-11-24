package com.db4o.drs.test.data;

/**
 * @sharpen.ignore
 */
public enum SimpleEnum {
    ONE(1),
    TWO(2);
    
    private int value;

    private SimpleEnum(int value) {
        this.value = value;
    }
    
    public void setValue(int value) {
        this.value = value;
    }
    
    public int getValue() {
        return value;
    }
}