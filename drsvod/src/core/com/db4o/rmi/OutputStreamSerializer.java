package com.db4o.rmi;

import java.io.*;
import java.util.*;

final class OutputStreamSerializer implements Serializer<OutputStream> {
    private final Distributor<?> distributor;

    OutputStreamSerializer(Distributor<?> distributor) {
        this.distributor = distributor;
    }

    public void serialize(DataOutput out, final OutputStream value) throws IOException {

        OutputStreamProxy proxy = new OutputStreamProxy() {
            
            public void write(byte[] buffer) throws IOException {
                value.write(buffer);
            }
        };
        out.writeLong(distributor.serverFor(proxy).getId());
    }

    public OutputStream deserialize(final DataInput in) throws IOException {
        final OutputStreamProxy proxy = distributor.proxyFor(in.readLong(), OutputStreamProxy.class).sync();
        return new OutputStream() {

            @Override
            public void write(int b) throws IOException {
                byte[] one = new byte[] {(byte)b};
                write(one);
            }
            
            @Override
            public void write(byte[] b) throws IOException {
                proxy.write(b);
            }
            
            @Override
            public void write(byte[] b, int off, int len) throws IOException {
                if (off == 0 && b.length == len) {
                    write(b);
                    return;
                }
                write(Arrays.copyOfRange(b, off, len+off));
            }
            
        };
    }
}
