/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.performance;

import java.io.*;

import com.db4o.io.*;

/**
 * 
 * @exclude
 */
public class RunIOBench {
	public static void main(String[] args) throws IOException {

		RandomAccessFile recordedIn = new RandomAccessFile(Util.BENCHFILE+".1", "rw");
		new File(Util.DBFILE).delete();
		Bin testbin = new FileStorage().open(new BinConfiguration(Util.DBFILE, false, 1024, false));

		// IoAdapter testadapt = new MemoryIoAdapter().open(Util.DBFILE, false,
		// 1024);
		// IoAdapter testadapt = new SymbianIoAdapter().open(Util.DBFILE,
		// false, 1024);
		long bench = benchmark(recordedIn, testbin);
		System.out.println("tested IOAdapter: ["
				+ testbin.getClass().getName() + "]\nspeed: " + bench);
	}

	public static long benchmark(RandomAccessFile recordedIn, Bin bin) throws IOException {
		byte[] defaultData = new byte[1000];
		long start = System.currentTimeMillis();
		int runs = 0;
		try {
			while (true) {
				runs++;
				char type = recordedIn.readChar();
				if (type == 'q') {
					break;
				}
				if (type == 'f') {
					bin.sync();
					continue;
				}
				long pos = recordedIn.readLong();
				int length = recordedIn.readInt();
				byte[] data = (length <= defaultData.length ? defaultData
						: new byte[length]);
				switch (type) {
				case 'r':
					bin.read(pos, data, length);
					break;
				case 'w':
					bin.write(pos, data, length);
					break;
				default:
					throw new IllegalArgumentException("Unknown access type: "
							+ type);
				}
			}
		} finally {
			recordedIn.close();
			bin.close();
		}
		// System.err.println(runs);
		return System.currentTimeMillis() - start;
	}
}