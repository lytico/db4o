package com.db4o.drs.test.data;

/**
 * @sharpen.remove
 */
public class SimpleEnumContainer {
    
    private SimpleEnum value;
    
    private int opaque;
    
    public SimpleEnumContainer() {
    }
    
    public SimpleEnumContainer(SimpleEnum myEnum) {
        this.value = myEnum;
    }

    public void setValue(SimpleEnum myEnum) {
        this.value = myEnum;
    }
    
    public SimpleEnum getValue() {
        return value;
    }
    
    public int getOpaque() {
        return opaque;
    }
    
    public void setOpaque(int opaque) {
        this.opaque = opaque;
    }
    
}