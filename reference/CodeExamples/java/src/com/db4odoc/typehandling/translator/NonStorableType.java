package com.db4odoc.typehandling.translator;

/**
 * This is our example class which represents a not storable type
 */
class NonStorableType {
    private String data;
    private transient int dataLength = 0;

    public NonStorableType(String data) {
        this.data = data;
        this.dataLength = data.length();
    }

    public String getData() {
        return data;
    }

    public int getDataLength() {
        return dataLength;
    }

    public void setData(String data) {
        this.data = data;
        this.dataLength = data.length();
    }
}
