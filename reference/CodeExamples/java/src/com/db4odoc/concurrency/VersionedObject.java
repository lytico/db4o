package com.db4odoc.concurrency;

/**
* @author roman.stoffel@gamlor.info
* @since 16.09.2010
*/
public class VersionedObject {
    private String data;
    private long version;

    public VersionedObject(String data) {
        this.data = data;
    }

    public String getData() {
        return data;
    }

    public long getVersion() {
        return version;
    }

    public void setVersion(long version) {
        this.version = version;
    }

    @Override
    public String toString() {
        return "VersionedObject{" +
                "version="+getVersion()+
                '}';
    }
}
