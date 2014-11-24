package com.db4odoc.configuration.io;

import com.db4o.io.Bin;
import com.db4o.io.BinDecorator;

// #example: A logging bin decorator
public class LoggingBin extends BinDecorator implements Bin{
    public LoggingBin(Bin bin) {
        super(bin);
    }

    @Override
    public void close() {
        System.out.println("Called LoggingBin.close()");
        super.close();
    }

    @Override
    public long length() {
        System.out.println("Called LoggingBin.length()");
        return super.length();
    }

    @Override
    public int read(long position, byte[] bytes, int bytesToRead) {
        System.out.println("Called LoggingBin.read("+position+", ...,"+bytesToRead+")");
        return super.read(position, bytes, bytesToRead);
    }

    @Override
    public void sync() {
        System.out.println("Called LoggingBin.sync()");
        super.sync();
    }

    @Override
    public int syncRead(long position, byte[] bytes, int bytesToRead) {
        System.out.println("Called LoggingBin.syncRead("+position+", ...,"+bytesToRead+")");
        return super.syncRead(position, bytes, bytesToRead);
    }

    @Override
    public void write(long position, byte[] bytes, int bytesToWrite) {
        System.out.println("Called LoggingBin.write("+position+", ...,"+bytesToWrite+")");
        super.write(position, bytes, bytesToWrite);
    }

    @Override
    public void sync(Runnable runnable) {
        System.out.println("Called LoggingBin.sync("+runnable+")");
        super.sync(runnable);
    }
}
// #end example
