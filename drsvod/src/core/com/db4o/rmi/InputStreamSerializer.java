package com.db4o.rmi;

import java.io.*;
import java.util.*;

final class InputStreamSerializer implements Serializer<InputStream> {
    private final Distributor<?> distributor;

    InputStreamSerializer(Distributor<?> distributor) {
        this.distributor = distributor;
    }

    public void serialize(DataOutput out, final InputStream value) throws IOException {

        InputStreamProxy proxy = new InputStreamProxy() {
            public byte[] read(int len) throws IOException {
                byte[] buffer = new byte[len];
                int read = value.read(buffer);
                return read == -1 ? null : read == len ? buffer : Arrays.copyOf(buffer, read);
            }

        };
        ServerObject server = this.distributor.serverFor(proxy);
        out.writeLong(server.getId());
    }

    public InputStream deserialize(final DataInput in) throws IOException {
        
        final InputStreamProxy proxy = this.distributor.proxyFor(in.readLong(), InputStreamProxy.class).sync();
        return new InputStream() {

            @Override
            public int read() throws IOException {
                byte[] one = new byte[1];
                int r = read(one, 0, 1);
                return r == -1 ? r : one[0];
            }

            @Override
            public int read(byte[] b) throws IOException {
                return read(b, 0, b.length);
            }

            @Override
            public int read(byte[] b, int off, int len) throws IOException {
                byte[] buffer = proxy.read(len);
                if (buffer == null) {
                    return -1;
                }
                System.arraycopy(buffer, 0, b, off, buffer.length);
                return buffer.length;
            }
        };
    }
}
