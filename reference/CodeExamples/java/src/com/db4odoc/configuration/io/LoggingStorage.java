package com.db4odoc.configuration.io;

import com.db4o.ext.Db4oIOException;
import com.db4o.io.Bin;
import com.db4o.io.BinConfiguration;
import com.db4o.io.Storage;
import com.db4o.io.StorageDecorator;

import java.io.IOException;

// #example: A logging storage decorator
public class LoggingStorage extends StorageDecorator implements Storage{
    public LoggingStorage(Storage storage) {
        super(storage);
    }

    @Override
    public boolean exists(String uri) {
        System.out.println("Called: LoggingStorage.exists("+uri+")");
        return super.exists(uri);
    }

    @Override
    public Bin open(BinConfiguration config) throws Db4oIOException {
        System.out.println("Called: LoggingStorage.open("+config+")");
        return super.open(config);
    }

    @Override
    protected Bin decorate(BinConfiguration config, Bin bin) {
        return new LoggingBin(super.decorate(config, bin));
    }

    @Override
    public void delete(String uri) throws IOException {
        System.out.println("Called: LoggingStorage.delete("+uri+")");
        super.delete(uri);
    }

    @Override
    public void rename(String oldUri, String newUri) throws IOException {
        System.out.println("Called: LoggingStorage.rename("+oldUri+","+newUri+")");
        super.rename(oldUri, newUri);
    }
}
//#end example
